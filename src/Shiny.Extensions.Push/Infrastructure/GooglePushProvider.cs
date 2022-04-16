using Shiny.Extensions.Push.Providers;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class GooglePushProvider : IGooglePushProvider
    {
        const string FcmUrl = "https://fcm.googleapis.com/fcm/send";
        readonly HttpClient httpClient = new HttpClient();


        public virtual GoogleNotification CreateNativeNotification(GoogleConfiguration configuration, Notification notification)
        {
            var native = new GoogleNotification
            {
                Data = notification.Data,
                Notification = new()
                {
                    Title = notification.Title,
                    Body = notification.Message,
                    Image = notification.ImageUri
                },
                Android = new()
                {
                    //TimeToLive = notification.Expiration?.TotalSeconds + "s",
                    //CollapseKey = null,
                    //Priority = null,
                    //RestrictedPackageName = null,
                    //DirectBootOk = true,

                    Notification = new()
                    {
                        ChannelId = configuration.DefaultChannelId,
                        Title = notification.Title,
                        Body = notification.Message,
                        ImageUrl = notification.ImageUri
                    }
                }
            };
            return native;
        }


        public virtual async Task<bool> Send(GoogleConfiguration configuration, string deviceToken, Notification notification, GoogleNotification native, CancellationToken cancelToken = default)
        {
            configuration.AssertValid();

            native.To = deviceToken;
            native.Token = deviceToken;
            var json = Serializer.Serialize(native);

            using (var request = new HttpRequestMessage(HttpMethod.Post, FcmUrl))
            {
                request.Headers.Add("Authorization", $"key = {configuration.ServerKey}");
                request.Headers.Add("Sender", $"id = {configuration.SenderId}");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await this.httpClient.SendAsync(request, cancelToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);
                if (String.IsNullOrWhiteSpace(responseString))
                    throw new ArgumentException("No response from firebase");

                var result = Serializer.DeserialzeFcmResponse(responseString)!;
                return result.Success == 1 && result.Failure == 0;
            }
        }
    }
}