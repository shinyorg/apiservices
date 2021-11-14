using CorePush.Apple;

using System.Threading.Tasks;


namespace Shiny.Api.Push.Providers
{
    public class ApplePushProvider : IPushProvider
    {
        readonly ApnSender apnSender;


        public ApplePushProvider()
        {
            this.apnSender = new ApnSender(
                new ApnSettings
                {
                    //AppBundleIdentifier = TestStartup.CurrentPlatform.AppIdentifier, // com.shiny.test
                    //ServerType = ApnServerType.Development,
                    //P8PrivateKey = Secrets.Values.ApnPrivateKey,
                    //P8PrivateKeyId = Secrets.Values.ApnPrivateKeyId,
                    //TeamId = Secrets.Values.ApnTeamId
                },
                new System.Net.Http.HttpClient()
            );
        }


        public PushPlatform Platform { get; } = PushPlatform.Apple;

        public async Task Send(string deviceToken, Notification notification)
        {
            var apple = new AppleNotification
            {
                AlertBody = new AppleNotification.Alert
                {

                }
            };
            if (notification.Data != null)
            {
                foreach (var item in notification.Data)
                {
                    apple.Add(item.Key, item.Value);
                }
                apple.ContentAvailable = 1;
            }
            await this.apnSender.SendAsync(apple, deviceToken); // expiration, priority
        }
    }
}
