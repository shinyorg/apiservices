using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.Extensions.Push.Providers;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class PushManager : IPushManager
    {
        readonly IRepository repository;
        readonly IApplePushProvider? apple;
        readonly IGooglePushProvider? google;
        readonly IAppleConfigurationProvider? appleConfigurationProvider = null;
        readonly IGoogleConfigurationProvider? googleConfigurationProvider = null;
        readonly List<IAppleNotificationDecorator> appleDecorators;
        readonly List<IGoogleNotificationDecorator> googleDecorators;
        readonly List<INotificationReporter> reporters;
        readonly ILogger logger;


        public PushManager(IRepository repository,
                           ILogger<PushManager> logger,
                           IEnumerable<INotificationReporter> reporters,
                           IEnumerable<IAppleNotificationDecorator> appleDecorators,
                           IEnumerable<IGoogleNotificationDecorator> googleDecorators,
                           IAppleConfigurationProvider? appleConfigurationProvider = null,
                           IGoogleConfigurationProvider? googleConfigurationProvider = null,
                           IApplePushProvider? apple = null,
                           IGooglePushProvider? google = null)
        {
            if (apple == null && google == null)
                throw new ArgumentException("No push provider has been registered");

            if (apple != null && appleConfigurationProvider == null)
                throw new ArgumentException("Apple Push Provider has been registered without an Apple Configuration Provider has been registered");

            if (google != null && googleConfigurationProvider == null)
                throw new ArgumentException("Google Push Provider has been registered without an Google Configuration Provider has been registered");

            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.apple = apple;
            this.google = google;
            this.appleDecorators = appleDecorators.ToList();
            this.googleDecorators = googleDecorators.ToList();
            this.appleConfigurationProvider = appleConfigurationProvider;
            this.googleConfigurationProvider = googleConfigurationProvider;
            this.reporters = reporters.ToList();
        }


        public Task<IEnumerable<PushRegistration>> GetRegistrations(PushFilter? filter, CancellationToken cancelToken = default)
            => this.repository.Get(filter, cancelToken);


        public Task Register(PushRegistration registration, CancellationToken cancelToken = default)
        {
            if (registration.Platform == PushPlatforms.All)
                throw new ArgumentException("You can only register a single platform at a time");

            return this.repository.Save(registration, cancelToken);
        }


        public async Task Send(Notification notification, PushFilter? filter, int maxParallelization = 3, CancellationToken cancelToken = default)
        {
            notification = notification ?? throw new ArgumentException("Notification is null");
            var registrations = (await this.repository.Get(filter, cancelToken)
                .ConfigureAwait(false))
                .ToArray();

            await this
                .Send(notification, registrations, maxParallelization, cancelToken)
                .ConfigureAwait(false);
        }


        public async Task Send(Notification notification, PushRegistration[] registrations, int maxParallelization = 3, CancellationToken cancelToken = default)
        {
            notification = notification ?? throw new ArgumentException("Notification is null");

            var context = new NotificationBatchContext(this.logger, this.reporters, notification, cancelToken);
            await context.OnBatchStart(registrations).ConfigureAwait(false);

            await Parallel
                .ForEachAsync(
                    registrations, 
                    new ParallelOptions 
                    {  
                        CancellationToken = cancelToken, 
                        MaxDegreeOfParallelism = maxParallelization
                    }, 
                    async (reg, ct) =>
                    {
                        try
                        {
                            var result = false;
                            switch (reg.Platform)
                            {
                                case PushPlatforms.Apple:
                                    result = await this.DoApple(context, reg, notification, cancelToken).ConfigureAwait(false);
                                    break;

                                case PushPlatforms.Google:
                                    result = await this.DoGoogle(context, reg, notification, cancelToken).ConfigureAwait(false);
                                    break;
                            }
                            if (result)
                            {
                                await context
                                    .OnNotificationSuccess(reg)
                                    .ConfigureAwait(false);
                            }
                            else
                            {
                                await context
                                    .OnNotificationError(reg, NoSendException.Instance)
                                    .ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            await context
                                .OnNotificationError(reg, ex)
                                .ConfigureAwait(false);
                        }
                    }
                )
                .ConfigureAwait(false);

            await context
                .OnBatchCompleted()
                .ConfigureAwait(false);
        }


        public Task UnRegister(PushPlatforms platform, string deviceToken, CancellationToken cancelToken = default)
        {
            if (String.IsNullOrEmpty(deviceToken))
                throw new ArgumentNullException(nameof(deviceToken));

            if (platform == PushPlatforms.All)
                throw new ArgumentException("You can only unregister on one platform when using device token");

            return this.repository.Remove(
                new PushFilter
                {
                    Platform = platform,
                    DeviceToken = deviceToken
                }, 
                cancelToken
            );
        }


        public Task UnRegisterByUser(string userId, CancellationToken cancelToken = default)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            return this.repository.Remove(new PushFilter { UserId = userId }, cancelToken);
        }


        async Task<bool> DoApple(NotificationBatchContext context, PushRegistration registration, Notification notification, CancellationToken cancelToken)
        {
            if (this.apple == null)
                throw new ArgumentException("Apple Push is not registered with this manager");

            context.AppleConfiguration ??= await this.appleConfigurationProvider!
                .GetConfiguration(notification)
                .ConfigureAwait(false);
            var appleNative = this.apple.CreateNativeNotification(context.AppleConfiguration, notification);

            await Task
                .WhenAll(this.appleDecorators
                    .Select(x => x.Decorate(registration, notification!, appleNative, cancelToken))
                    .ToArray()
                )
                .ConfigureAwait(false);

            if (notification!.DecorateApple != null)
                await notification.DecorateApple.Invoke(registration, appleNative);

            return await this.apple
                .Send(context.AppleConfiguration, registration.DeviceToken, notification, appleNative, cancelToken)
                .ConfigureAwait(false);
        }


        async Task<bool> DoGoogle(NotificationBatchContext context, PushRegistration registration, Notification notification, CancellationToken cancelToken = default)
        {
            if (this.google == null)
                throw new ArgumentException("No Google provider is registered with this manager");

            context.GoogleConfiguration ??= await this.googleConfigurationProvider!
                .GetConfiguration(notification)
                .ConfigureAwait(false);

            var googleNative = this.google.CreateNativeNotification(context.GoogleConfiguration, notification);
            await Task
                .WhenAll(this.googleDecorators
                    .Select(x => x.Decorate(registration, notification!, googleNative, cancelToken))
                    .ToArray()
                )
                .ConfigureAwait(false);

            if (notification!.DecorateGoogle != null)
                await notification.DecorateGoogle.Invoke(registration, googleNative);

            return await this.google
                .Send(context.GoogleConfiguration, registration.DeviceToken, notification, googleNative, cancelToken)
                .ConfigureAwait(false);
        }
    }
}