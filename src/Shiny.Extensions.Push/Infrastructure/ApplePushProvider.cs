using Shiny.Extensions.Push.Providers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class ApplePushProvider : IApplePushProvider
    {
        const string DevUrl = "https://api.development.push.apple.com";
        const string ProdUrl = "https://api.push.apple.com";

        readonly HttpClient httpClient;
        readonly IAppleAuthTokenProvider authTokenProvider;


        public ApplePushProvider(IAppleAuthTokenProvider authTokenProvider)
        {
            this.httpClient = new HttpClient();
            this.authTokenProvider = authTokenProvider;
        }


        public virtual AppleNotification CreateNativeNotification(AppleConfiguration config, Notification notification)
        {
            var native = new AppleNotification
            {
                Aps = new Aps
                {
                    Category = notification.CategoryOrChannel
                }
            };
            if (notification.Title != null || notification.Message != null)
            {
                native.Aps.Alert = new ApsAlert
                {
                    Title = notification.Title,
                    Body = notification.Message,
                    LaunchImage = notification.ImageUri
                };
            }
            if (notification.Data != null)
            {
                foreach (var pair in notification.Data)
                {
                    native.Add(pair.Key, pair.Value);
                }
                native.Aps.ContentAvailable = 1;
            }
            return native;
        }


        public virtual async Task<bool> Send(AppleConfiguration config, string deviceToken, Notification notification, AppleNotification native, CancellationToken cancelToken = default)
        {
            config.AssertValid();

            var path = "/3/device/" + deviceToken;
            var url = (config.IsProduction ? ProdUrl : DevUrl) + path;
            var json = Serializer.Serialize(native);

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
            {
                Version = new Version(2, 0),
                Content = new StringContent(json)
            };

            var jwt = this.authTokenProvider.GetAuthToken(config);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", jwt);
            request.Headers.TryAddWithoutValidation(":method", "POST");
            request.Headers.TryAddWithoutValidation(":path", path);
            request.Headers.TryAddWithoutValidation("apns-id", Guid.NewGuid().ToString("D"));
            request.Headers.TryAddWithoutValidation("apns-topic", config.AppBundleIdentifier);
            //apns-collapse-id
            //notification.Expiration == null ? "0" : from epoch
            request.Headers.TryAddWithoutValidation("apns-expiration", Convert.ToString(0));

            var silentPush = native.Aps.Alert == null && native.Aps.ContentAvailable == 1;
            request.Headers.Add("apns-priority", silentPush ? "5" : "10");

            // for iOS 13 required - TODO: more new types here, need a way to configure headers as well - perhaps non-serialized props in native notification?
            request.Headers.Add("apns-push-type", silentPush ? "background" : "alert");

            var response = await this.httpClient.SendAsync(request, cancelToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return true;

            var content = await response.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);
            var apnResponse = Serializer.DeserialzeAppleResponse(content);
            var reason = apnResponse?.Reason;

            if (reason == "BadDeviceToken" || reason == "Unregistered")
                return false;

            throw new ApnException(reason);
            // TODO: retry reasons
            // ExpiredProviderToken
            // InternalServerError,
            // ServiceUnavailable,
            // Shutdown, 
        }
    }
}

//BadCollapseId,
//        BadDeviceToken,
//        BadExpirationDate,
//        BadMessageId,
//        BadPriority,
//        BadTopic,
//        DeviceTokenNotForTopic,
//        DuplicateHeaders,
//        IdleTimeout,
//        MissingDeviceToken,
//        MissingTopic,
//        PayloadEmpty,
//        TopicDisallowed,
//        BadCertificate,
//        BadCertificateEnvironment,
//        ExpiredProviderToken,
//        Forbidden,
//        InvalidProviderToken,
//        MissingProviderToken,
//        BadPath,
//        MethodNotAllowed,
//        Unregistered,
//        PayloadTooLarge,
//        TooManyProviderTokenUpdates,
//        TooManyRequests,
//        InternalServerError,
//        ServiceUnavailable,
//        Shutdown, 