using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Shiny.Extensions.Push.Infrastructure;

namespace Shiny.Extensions.Push.Apple.Infrastructure;


public class ApplePushProvider : IPushProvider
{
    const string DevUrl = "https://api.development.push.apple.com";
    const string ProdUrl = "https://api.push.apple.com";

    readonly AppleConfiguration config;
    readonly IEnumerable<IAppleEvents> events;
    readonly HttpClient httpClient = new();


    public ApplePushProvider(AppleConfiguration config, IEnumerable<IAppleEvents> events)
    {
        this.config = config;
        this.events = events;
    }


    public bool CanPushTo(PushRegistration registration)
    {
        if (registration.Platform.Equals("apple", StringComparison.InvariantCultureIgnoreCase))
            return true;

        if (registration.Platform.Equals("ios", StringComparison.InvariantCultureIgnoreCase))
            return true;

        return false;
    }


    public async Task Send(INotification notification, PushRegistration registration, CancellationToken cancellationToken)
    {
        var path = $"/v3/device/{registration.DeviceToken}";
        var url = (this.config.IsProduction ? ProdUrl : DevUrl) + path;

        var native = await this.CreateNativeNotification(registration, notification);
        var json = Serializer.Serialize(new AppleNotification { });

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
        {
            Version = new Version(2, 0),
            Content = new StringContent(json)
        };

        var jwt = this.GetAuthToken(this.config);

        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", jwt);
        request.Headers.TryAddWithoutValidation(":method", "POST");
        request.Headers.TryAddWithoutValidation(":path", path);
        request.Headers.TryAddWithoutValidation("apns-id", native.ApnsId ?? Guid.NewGuid().ToString("D"));
        request.Headers.TryAddWithoutValidation("apns-topic", this.GetApnsTopic(this.config.AppBundleIdentifier, native));
        request.Headers.TryAddWithoutValidation("apns-expiration", Convert.ToString(native.ExpirationFromEpoch ?? 0));

        if (native.ApnsCollapseId != null)
            request.Headers.TryAddWithoutValidation("apns-collapse-id", native.ApnsCollapseId);

        if (native.ApnsPriority != null) 
            request.Headers.Add("apns-priority", native.ApnsPriority.ToString());

        if (native.PushType != null)
            request.Headers.TryAddWithoutValidation("apns-push-type", native.PushType.ToString().ToLower());

        else if (native.Aps?.ContentAvailable == 1 && native.Aps?.Alert == null)
            request.Headers.TryAddWithoutValidation("apns-push-type", "background");

        else
            request.Headers.TryAddWithoutValidation("apns-push-type", "alert");


        var response = await this.httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var apnResponse = Serializer.DeserialzeAppleResponse(content);
            var reason = apnResponse?.Reason;

            // TODO: should cause registration to be removed
            if (reason == "BadDeviceToken" || reason == "Unregistered")
                return;

            throw new ApnException(reason);
            // TODO: retry reasons
            // ExpiredProviderToken
            // InternalServerError,
            // ServiceUnavailable,
            // Shutdown,
        }
    }


    protected virtual string GetApnsTopic(string appBundleIdentifier, AppleNotification native)
    {
        if (native.ApnsTopic != null)
            return native.ApnsTopic;

        if (native.PushType == PushType.Location)
            return $"{appBundleIdentifier}.location-query";

        if (native.PushType == PushType.FileProvider)
            return $"{appBundleIdentifier}pushkit.fileprovider";

        return appBundleIdentifier;
    }


    protected virtual async Task<AppleNotification> CreateNativeNotification(PushRegistration registration, INotification notification)
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
            native.CustomData = new();
            foreach (var pair in notification.Data)
            {
                native.CustomData.Add(pair.Key, pair.Value);
            }
            native.Aps.ContentAvailable = 1;
        }
        if (this.events.Any())
            await Task.WhenAll(this.events.Select(x => x.OnBeforeSend(notification, registration, native)));

        if (notification is IAppleNotificationCustomizer customizer && customizer.AppleBeforeSend != null)
            await customizer.AppleBeforeSend.Invoke(native, registration);

        return native;
    }


    readonly Dictionary<string, JwtToken> tokens = new();
    readonly object syncLock = new object();


    public virtual string GetAuthToken(AppleConfiguration config)
    {
        if (!this.tokens.ContainsKey(config.TeamId) || this.tokens[config.TeamId].Expires < DateTime.UtcNow)
        {
            lock (this.syncLock)
            {
                if (!this.tokens.ContainsKey(config.TeamId) || this.tokens[config.TeamId].Expires < DateTime.UtcNow)
                {
                    this.tokens.Remove(config.TeamId);
                    var tokenValue = this.CreateJwtToken(config);
                    this.tokens.Add(config.TeamId, new JwtToken(tokenValue, DateTime.UtcNow.AddMinutes(config.JwtExpiryMinutes))); // cannot be less than 20 or great than 60
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }
        return this.tokens[config.TeamId].Value;
    }


    protected virtual string CreateJwtToken(AppleConfiguration config)
    {
        var privateKey = this.GetECDsa(config.Key);
        var securityKey = new ECDsaSecurityKey(privateKey) { KeyId = config.KeyId };
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            Issuer = config.TeamId,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var encodedToken = handler.CreateEncodedJwt(descriptor);
        return encodedToken;
    }


    protected ECDsa GetECDsa(string key)
    {
        using var reader = new StringReader(key);
        var pemReader = new PemReader(reader);
        var ecPrivateKeyParameters = (ECPrivateKeyParameters)pemReader.ReadObject();

        var q = ecPrivateKeyParameters.Parameters.G.Multiply(ecPrivateKeyParameters.D).Normalize();
        var qx = q.AffineXCoord.GetEncoded();
        var qy = q.AffineYCoord.GetEncoded();
        var d = ecPrivateKeyParameters.D.ToByteArrayUnsigned();

        // Convert the BouncyCastle key to a Native Key.
        var msEcp = new ECParameters { Curve = ECCurve.NamedCurves.nistP256, Q = { X = qx, Y = qy }, D = d };
        return ECDsa.Create(msEcp);
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