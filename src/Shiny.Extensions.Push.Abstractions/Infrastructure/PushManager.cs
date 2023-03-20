using System;
using Microsoft.Extensions.Logging;

namespace Shiny.Extensions.Push.Infrastructure;


public class PushManager : IPushManager
{
    readonly IPushRepository repository;
    readonly ILogger logger;
    readonly IEnumerable<IPushProvider> providers;
    readonly IEnumerable<INotificationReporter> reporters;


    public PushManager(
        IPushRepository repository,
        ILogger<PushManager> logger,
        IEnumerable<IPushProvider> providers,
        IEnumerable<INotificationReporter> reporters
    )
	{
        this.repository = repository;
        this.logger = logger;
        this.providers = providers;
        this.reporters = reporters;
	}

    public Task<IList<PushRegistration>> GetRegistrations(Filter? filter, CancellationToken cancellationToken = default)
        => this.repository.Get(filter, cancellationToken);

    public Task Register(PushRegistration registration)
    {
        throw new NotImplementedException();
    }

    public Task Send(INotification notification, Filter? filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Send(INotification notification, PushRegistration[] registrations, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UnRegister(string platform, string deviceToken, CancellationToken cancelToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UnRegister(string userId, CancellationToken cancelToken = default)
    {
        throw new NotImplementedException();
    }
}

/*


    public Task<IEnumerable<PushRegistration>> GetRegistrations(PushFilter? filter, CancellationToken cancelToken = default)
        => this.repository.Get(filter, cancelToken);


    public Task Register(PushRegistration registration, CancellationToken cancelToken = default)
    {
        if (registration.Platform == PushPlatforms.All)
            throw new ArgumentException("You can only register a single platform at a time");

        return this.repository.Save(registration, cancelToken);
    }


    public async Task Send(Notification notification, PushFilter? filter, CancellationToken cancelToken = default)
    {
        notification = notification ?? throw new ArgumentException("Notification is null");
        var registrations = (await this.repository.Get(filter, cancelToken)
            .ConfigureAwait(false))
            .ToArray();

        await this
            .Send(notification, registrations, cancelToken)
            .ConfigureAwait(false);
    }


    public async Task Send(Notification notification, PushRegistration[] registrations, CancellationToken cancelToken = default)
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
                    MaxDegreeOfParallelism = this.MaxParallelization
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
 
 */