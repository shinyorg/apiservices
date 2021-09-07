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
        public Task Send(Notification notification)
        {
            throw new System.NotImplementedException();
        }
    }
}
