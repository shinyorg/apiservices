using System.Text;
using Shiny.Extensions.Push.Infrastructure;

namespace Shiny.Extensions.Push.GoogleFirebase.Infrastructure;


public class GooglePushProvider : IPushProvider
{
    const string FcmUrl = "https://fcm.googleapis.com/fcm/send";
    readonly HttpClient httpClient = new HttpClient();

    readonly GoogleConfiguration config;
    readonly IEnumerable<IGoogleEvents> events;


    public GooglePushProvider(GoogleConfiguration config, IEnumerable<IGoogleEvents> events)
    {
        this.config = config;
        this.events = events;
    }


    public bool CanPushTo(PushRegistration registration) => this.CanPushTo(registration.Platform);

    public bool CanPushTo(string platform)
    {
        if (platform.Equals("google", StringComparison.InvariantCultureIgnoreCase))
            return true;

        return false;
    }

    public async Task<bool> Send(INotification notification, PushRegistration registration, CancellationToken cancelToken)
    {
        var native = await this.CreateNativeNotification(notification, registration);
        var json = Serializer.Serialize(native);

        using var request = new HttpRequestMessage(HttpMethod.Post, FcmUrl);
        request.Headers.Add("Authorization", $"key = {this.config.ServerKey}");
        request.Headers.Add("Sender", $"id = {this.config.SenderId}");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.httpClient.SendAsync(request, cancelToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);
        if (String.IsNullOrWhiteSpace(responseString))
            throw new ArgumentException("No response from firebase");

        var result = Serializer.DeserialzeFcmResponse(responseString)!;
        return (result.Success == 1) ;
    }


    protected async virtual Task<GoogleNotification> CreateNativeNotification(INotification notification, PushRegistration registration)
    {
        var native = new GoogleNotification
        {
            To = registration.DeviceToken,
            Token = registration.DeviceToken,
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
                    ChannelId = this.config.DefaultChannelId,
                    Title = notification.Title,
                    Body = notification.Message,
                    ImageUrl = notification.ImageUri
                }
            }
        };

        foreach (var e in this.events)
            await e.OnBeforeSend(notification, registration, native);

        if (notification is IGoogleNotificationCustomizer customizer && customizer.GoogleBeforeSend != null)
            await customizer.GoogleBeforeSend(native, registration);

        return native;
    }
}