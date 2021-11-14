using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;


namespace Shiny.Api.Push.Providers
{
    public class GooglePushProvider : IPushProvider
    {
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


        public PushPlatform Platform { get; } = PushPlatform.Google;

        public async Task Send(string deviceToken, Notification notification)
        {
            var msg = new Message
            {
                //Apns
                //FcmOptions =
                //Condition
                //Topic =
                //Webpush
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Body = "",
                        BodyLocKey = "",
                        //BodyLocArgs = "",
                        Icon = "",
                        Tag = "",
                        ImageUrl = "",

                        Title = "",
                        TitleLocKey = "",
                        TitleLocArgs = new [] { "" },

                        ClickAction = "",

                        ChannelId = "",
                        Color = "",
                        Sound = ""
                    },
                    //RestrictedPackageName
                    //TimeToLive
                    Priority = Priority.Normal
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Alert = new ApsAlert
                        {
                            ActionLocKey = "",

                            LocKey = "",
                            LocArgs = new [] { "" },

                            Title = "",
                            TitleLocKey = "",
                            TitleLocArgs = new [] { "" },
                            //Subtitle = ""
                            Subtitle = "",
                            SubtitleLocKey = "",
                            SubtitleLocArgs = new [] { "" },

                            Body = notification.Message,
                            LaunchImage = ""
                        },
                        Badge = 0,
                        //MutableContent = true
                        ContentAvailable = true,
                        //CriticalSound = null
                        CustomData = null,
                        Sound = null,
                        ThreadId = "",
                        Category = notification.CategoryOrChannel
                    },
                    CustomData = null,
                    Headers = null
                },
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notification.Title,
                    Body = notification.Message,
                    ImageUrl = notification.ImageUri
                },
                Token = deviceToken,
                //Data = notification.Data
            };
            await FirebaseMessaging.DefaultInstance.SendAsync(msg);
        }
    }
}
