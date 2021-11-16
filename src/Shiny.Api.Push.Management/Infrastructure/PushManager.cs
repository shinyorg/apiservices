using System.Collections.Generic;
using System.Threading.Tasks;
using Shiny.Api.Push.Management.Providers;


namespace Shiny.Api.Push.Management.Infrastructure
{
    public class PushManager : IPushManager
    {
        readonly IRepository repository;
        readonly IApplePushProvider? apple;
        readonly IGooglePushProvider? google;


        public PushManager(IRepository repository, 
                           IApplePushProvider? apple = null, 
                           IGooglePushProvider? google = null)
        {
            this.repository = repository;
            this.apple = apple;
            this.google = google;
        }


        public Task<IEnumerable<NotificationRegistration>> GetRegistrations(PushFilter? filter)
            => this.repository.Get(filter);


        public Task Register(NotificationRegistration registration) 
            => this.repository.Save(registration);


        public async Task Send(Notification notification, PushFilter? filter)
        {
            var tokens = await this.repository.Get(filter).ConfigureAwait(false);

            // TODO: take a count of successful/failures
            foreach (var token in tokens)
            {
                switch (token.Platform)
                {
                    case PushPlatform.Apple:
                        var appleNative = new AppleNotification();
                        await this.apple.Send(appleNative).ConfigureAwait(false);
                        break;

                    case PushPlatform.Google:
                        var googleNative = new GoogleNotification();
                        await this.google.Send(googleNative).ConfigureAwait(false);
                        break;
                }
            }
        }


        public Task UnRegister(PushFilter filter)
            => this.repository.Remove(filter);
    }
}
