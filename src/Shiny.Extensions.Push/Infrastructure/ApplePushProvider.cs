using Shiny.Extensions.Push.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{ 
    public class ApplePushProvider : AbstractApplePushProvider
    {
        readonly AppleConfiguration config;


        public ApplePushProvider(AppleConfiguration config)
            => this.config = config ?? throw new ArgumentNullException(nameof(config));


        public override Task<bool> Send(string deviceToken, Notification notification, AppleNotification native, CancellationToken cancelToken = default)
            => this.Send(this.config, deviceToken, notification, native, cancelToken);
    }
}