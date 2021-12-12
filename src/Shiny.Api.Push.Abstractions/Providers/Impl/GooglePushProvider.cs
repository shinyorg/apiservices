using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shiny.Api.Push.Providers.Impl.Infrastructure;

namespace Shiny.Api.Push.Providers
{
    public class GooglePushProvider : IGooglePushProvider
    {
        const string FcmUrl = "https://fcm.googleapis.com/fcm/send";
        readonly GoogleConfiguration configuration;
        readonly HttpClient httpClient;


        public GooglePushProvider(GoogleConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }


        public GoogleNotification CreateNativeNotification(Notification notification)
        {
            var native = new GoogleNotification();

            return native;
        }


        //POST https://fcm.googleapis.com/v1/{parent=projects/*}/messages:send
        public async Task Send(string deviceToken, GoogleNotification notification)
        {
            var json = Serializer.Serialize(notification);

            using (var request = new HttpRequestMessage(HttpMethod.Post, FcmUrl))
            {
                request.Headers.Add("Authorization", $"key = {this.configuration.ServerKey}");
                request.Headers.Add("Sender", $"id = {this.configuration.SenderId}");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.SendAsync(request, CancellationToken.None);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                //return JsonHelper.Deserialize<FcmResponse>(responseString);
            }

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

            //            ClickAction = "", // always set to shiny push intent

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
