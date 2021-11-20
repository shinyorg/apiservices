using System.Threading.Tasks;


namespace Shiny.Api.Push.Providers
{
    public class ApplePushProvider : IApplePushProvider
    {
        //readonly ApnSender apnSender;


        public ApplePushProvider()
        {
            //this.apnSender = new ApnSender(
            //    new ApnSettings
            //    {
            //        //AppBundleIdentifier = TestStartup.CurrentPlatform.AppIdentifier, // com.shiny.test
            //        //ServerType = ApnServerType.Development,
            //        //P8PrivateKey = Secrets.Values.ApnPrivateKey,
            //        //P8PrivateKeyId = Secrets.Values.ApnPrivateKeyId,
            //        //TeamId = Secrets.Values.ApnTeamId
            //    },
            //    new System.Net.Http.HttpClient()
            //);
        }

        public AppleNotification CreateNativeNotification(Notification notification)
        {
            throw new System.NotImplementedException();
        }


        public async Task Send(string deviceToken, AppleNotification notification)
        {
            var apple = new AppleNotification
            {
                //AlertBody = new AppleNotification.Alert
                //{

                //}
            };
            //if (notification.Data != null)
            //{
            //    foreach (var item in notification.Data)
            //    {
            //        apple.Add(item.Key, item.Value);
            //    }
            //    apple.ContentAvailable = 1;
            //}
            //await this.apnSender.SendAsync(apple, deviceToken); // expiration, priority
        }
    }
}
//Apns = new ApnsConfig
//{
//    Aps = new Aps
//    {
//        Alert = new ApsAlert
//        {
//            ActionLocKey = "",

//            LocKey = "",
//            LocArgs = new[] { "" },

//            Title = "",
//            TitleLocKey = "",
//            TitleLocArgs = new[] { "" },
//            //Subtitle = ""
//            Subtitle = "",
//            SubtitleLocKey = "",
//            SubtitleLocArgs = new[] { "" },

//            Body = notification.Message,
//            LaunchImage = ""
//        },
//        Badge = 0,
//        //MutableContent = true
//        ContentAvailable = true,
//        //CriticalSound = null
//        CustomData = null,
//        Sound = null,
//        ThreadId = "",
//        Category = notification.CategoryOrChannel
//    },
//    CustomData = null,
//    Headers = null
//},