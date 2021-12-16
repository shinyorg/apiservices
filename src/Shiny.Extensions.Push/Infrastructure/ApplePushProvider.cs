namespace Shiny.Extensions.Push.Infrastructure;

using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Shiny.Extensions.Push.Providers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;


public class ApplePushProvider : IApplePushProvider
{
    const string DevUrl = "https://api.development.push.apple.com";
    const string ProdUrl = "https://api.push.apple.com";

    readonly HttpClient httpClient;
    readonly AppleConfiguration config;


    public ApplePushProvider(AppleConfiguration config)
    {
        this.config = config ?? throw new ArgumentNullException(nameof(config));
        this.httpClient = new HttpClient();
    }


    public AppleNotification CreateNativeNotification(Notification notification)
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


    public async Task Send(string deviceToken, Notification notification, AppleNotification native)
    {
        var path = "/3/device/" + deviceToken;
        var url = (this.config.IsProduction ? ProdUrl : DevUrl) + path;
        var json = Serializer.Serialize(native);

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
        {
            Version = new Version(2, 0),
            Content = new StringContent(json)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", this.GetJwtToken());
        request.Headers.TryAddWithoutValidation(":method", "POST");
        request.Headers.TryAddWithoutValidation(":path", path);
        request.Headers.TryAddWithoutValidation("apns-id", Guid.NewGuid().ToString("D"));
        request.Headers.TryAddWithoutValidation("apns-topic", this.config.AppBundleIdentifier);
        //apns-collapse-id
        //notification.Expiration == null ? "0" : from epoch
        request.Headers.TryAddWithoutValidation("apns-expiration", Convert.ToString(0));

        var silentPush = native.Aps.Alert == null && native.Aps.ContentAvailable == 1;
        request.Headers.Add("apns-priority", silentPush ? "5" : "10");
        request.Headers.Add("apns-push-type", silentPush ? "background" : "alert"); // for iOS 13 required

        var response = await this.httpClient.SendAsync(request, CancellationToken.None);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        // TODO: parse content
    }


    //https://stackoverflow.com/questions/43870237/how-to-implement-apple-token-based-push-notifications-using-p8-file-in-c

    object syncLock = new object();
    DateTime? expiryTime;
    string? currentJwt;


    string GetJwtToken()
    {
        if (this.expiryTime == null || this.expiryTime < DateTime.UtcNow)
        {
            lock (this.syncLock)
            {
                if (this.expiryTime == null || this.expiryTime < DateTime.UtcNow)
                {
                    this.currentJwt = this.CreateJwtToken();
                    this.expiryTime = DateTime.UtcNow;
                }
            }
        }
        return this.currentJwt!;
    }


    string CreateJwtToken()
    {
        var privateKey = this.GetECDsa();
        var securityKey = new ECDsaSecurityKey(privateKey) { KeyId = this.config.KeyId };
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.Now,
            Issuer = this.config.TeamId,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var encodedToken = handler.CreateEncodedJwt(descriptor);
        return encodedToken;
    }


    ECDsa GetECDsa()
    {
        var reader = new StringReader(this.config.Key);
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