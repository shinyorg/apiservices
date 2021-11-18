using CorePush.Google;

using System.Threading.Tasks;


namespace Shiny.Api.Push.Providers
{
    public class GooglePushProvider : IGooglePushProvider
    {
        readonly FcmSender sender;


        public GooglePushProvider()
        {
            //var creds = GoogleCredential.FromJson(JsonConvert.SerializeObject(new TestGoogleCredential
            //{
            //    ProjectId = Secrets.Values.GoogleCredentialProjectId,
            //    PrivateKeyId = Secrets.Values.GoogleCredentialPrivateKeyId,
            //    PrivateKey = Secrets.Values.GoogleCredentialPrivateKey,
            //    ClientId = Secrets.Values.GoogleCredentialClientId,
            //    ClientEmail = Secrets.Values.GoogleCredentialClientEmail,
            //    ClientCertUrl = Secrets.Values.GoogleCredentialClientCertUrl
            //}));
            //FirebaseApp.Create(new AppOptions
            //{
            //    Credential = creds
            //});
        }


        public async Task Send(string deviceToken, GoogleNotification notification)
        {
            //var msg = new Message
            //{
            //    //Apns
            //    //FcmOptions =
            //    //Condition
            //    //Topic =
            //    //Webpush
            //    Android = new AndroidConfig
            //    {
            //        Notification = new AndroidNotification
            //        {
            //            Body = "",
            //            BodyLocKey = "",
            //            //BodyLocArgs = "",
            //            Icon = "",
            //            Tag = "",
            //            ImageUrl = "",

            //            Title = "",
            //            TitleLocKey = "",
            //            TitleLocArgs = new[] { "" },

            //            ClickAction = "",

            //            ChannelId = "",
            //            Color = "",
            //            Sound = ""
            //        },
            //        //RestrictedPackageName
            //        //TimeToLive
            //        Priority = Priority.Normal
            //    },
            //    Notification = new FirebaseAdmin.Messaging.Notification
            //    {
            //        Title = notification.Title,
            //        Body = notification.Message,
            //        ImageUrl = notification.ImageUri
            //    },
            //    Token = deviceToken,
            //    //Data = notification.Data
            //};
            //await FirebaseMessaging.DefaultInstance.SendAsync(msg);
        }
    }
}
