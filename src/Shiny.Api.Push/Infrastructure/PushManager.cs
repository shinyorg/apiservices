using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shiny.Api.Push.Providers;


namespace Shiny.Api.Push.Infrastructure
{
    public class PushManager : IPushManager
    {
        readonly IRepository repository;
        readonly IApplePushProvider? apple;
        readonly IGooglePushProvider? google;
        readonly List<IAppleNotificationDecorator> appleDecorators;
        readonly List<IGoogleNotificationDecorator> googleDecorators;


        public PushManager(IRepository repository,
                           IEnumerable<IAppleNotificationDecorator> appleDecorators,
                           IEnumerable<IGoogleNotificationDecorator> googleDecorators,
                           IApplePushProvider? apple = null,
                           IGooglePushProvider? google = null)
        {
            this.repository = repository ?? throw new ArgumentException("Repository is null");
            this.appleDecorators = appleDecorators.ToList();
            this.googleDecorators = googleDecorators.ToList();

            if (apple == null && google == null)
                throw new ArgumentException("No push provider has been registered");

            this.apple = apple;
            this.google = google;
        }


        public Task<IEnumerable<PushRegistration>> GetRegistrations(PushFilter? filter)
            => this.repository.Get(filter);


        public Task Register(PushRegistration registration)
        { 
            if (registration.Platform == PushPlatforms.All)
                throw new ArgumentException("You can only register a single platform at a time");

            return this.repository.Save(registration);
        }


        public async Task Send(Notification notification, PushFilter? filter)
        {
            notification = notification ?? throw new ArgumentException("Notification is null");
            var registrations = await this.repository.Get(filter).ConfigureAwait(false);

            // TODO: log successful/failure?
            foreach (var registration in registrations)
            {
                switch (registration.Platform)
                {
                    case PushPlatforms.Apple:
                        await this.DoApple(registration, notification).ConfigureAwait(false);
                        break;

                    case PushPlatforms.Google:
                        await this.DoGoogle(registration, notification).ConfigureAwait(false);
                        break;
                }
            }
        }


        public Task UnRegister(PushFilter filter)
            => this.repository.Remove(filter);


        async Task DoApple(PushRegistration registration, Notification notification)
        {
            if (this.apple == null)
                throw new ArgumentException("Apple Push is not registered with this manager");

            var appleNative = this.apple.CreateNativeNotification(notification);
            await Task
                .WhenAll(this.appleDecorators
                    .Select(x => x.Decorate(registration, notification!, appleNative))
                    .ToArray()
                )
                .ConfigureAwait(false);

            if (notification!.DecorateApple != null)
                await notification.DecorateApple.Invoke(registration, appleNative);

            await this.apple
                .Send(registration.DeviceToken, appleNative)
                .ConfigureAwait(false);
        }


        async Task DoGoogle(PushRegistration registration, Notification notification)
        {
            var googleNative = new GoogleNotification();
            await Task
                .WhenAll(this.googleDecorators
                    .Select(x => x.Decorate(registration, notification!, googleNative))
                    .ToArray()
                )
                .ConfigureAwait(false);

            if (notification!.DecorateGoogle != null)
                await notification.DecorateGoogle.Invoke(registration, googleNative);

            await this.google
                .Send(registration.DeviceToken, googleNative)
                .ConfigureAwait(false);
        }
    }
}
