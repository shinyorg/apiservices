namespace Shiny.Api.Push.Infrastructure;

using Shiny.Api.Push.Providers;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


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
        // TODO: time-to-live is available on APN as well
        var native = new GoogleNotification
        {
            Data = notification.Data,
        };
        native.Android = new GoogleAndroidConfig
        {
            Notification = new GoogleAndroidNotificationDetails
            {
                ClickAction = this.configuration.UseShinyAndroidPushIntent ? Constants.ShinyPushAndroidIntent : null,
                ChannelId = this.configuration.DefaultChannelId,
                Title = notification.Title,
                Body = notification.Message,
                ImageUrl = notification.ImageUri
            }
        };
        return native;
    }


    //POST https://fcm.googleapis.com/v1/{parent=projects/*}/messages:send
    public async Task Send(string deviceToken, Notification notification, GoogleNotification native)
    {
        native.Token = deviceToken; // TODO: watch this - may have to clone the native especially if I start doing Task.WhenAll
        var json = Serializer.Serialize(native);

        using (var request = new HttpRequestMessage(HttpMethod.Post, FcmUrl))
        {
            request.Headers.Add("Authorization", $"key = {this.configuration.ServerKey}");
            request.Headers.Add("Sender", $"id = {this.configuration.SenderId}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.SendAsync(request, CancellationToken.None);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = Serializer.DeserialzeFcmResponse(responseString);

            // TODO: logging
            if (result == null)
                throw new ArgumentException("Invalid response from Firebase - result was empty");

            // TODO: log/process result
        }
    }
}