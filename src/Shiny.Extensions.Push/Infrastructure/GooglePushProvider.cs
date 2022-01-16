using Shiny.Extensions.Push.Providers;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class GooglePushProvider : AbstractGooglePushProvider
    {
        readonly GoogleConfiguration configuration;

        public GooglePushProvider(GoogleConfiguration configuration)
            => this.configuration = configuration;

        public override GoogleNotification CreateNativeNotification(Notification notification)
            => this.CreateNativeNotification(this.configuration, notification);

        public override Task<bool> Send(string deviceToken, Notification notification, GoogleNotification native, CancellationToken cancelToken = default)
            => this.Send(this.configuration, deviceToken, notification, native, cancelToken);
    }
}