using Shiny.Api.Push.Providers.Impl.Infrastructure;

using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace Shiny.Api.Push.Providers
{
    public class ApplePushProvider : IApplePushProvider
    {
        const string DevUrl = "https://api.development.push.apple.com";
        const string ProdUrl = "https://api.push.apple.com";
        readonly AppleConfiguration config;
        //readonly ApnSender apnSender;


        public ApplePushProvider(AppleConfiguration config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
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
            //int apnsExpiration = 0,
            //int apnsPriority = 10,
            //bool isBackground = false,
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


        //System.Security.Cryptography.Algorithms
        // KeyID: 29574YKVMC
        //----BEGIN PRIVATE KEY-----
        //MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgp2TcJ78OOZxf7uIo
        //y2ofKKQ9JuYcIVGNdERYqdkJB+OgCgYIKoZIzj0DAQehRANCAASkryWeqM1mDma3
        //aoIxHqPJJ1myIbpLTrrNv7Wy97Mvl2DFGfjxMqUVMciqv5amuoNbD+B0+X/SyDpM
        //Z8aRsaN0
        //-----END PRIVATE KEY-----

        public async Task Send(string deviceToken, AppleNotification notification)
        {
            var path = "/3/device/" + deviceToken;
            var url = (this.config.IsProduction ? ProdUrl : DevUrl) + path;
            var json = Serializer.Serialize(notification);
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
            {
                Version = new Version(2, 0),
                Content = new StringContent(json)
            };

            //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", GetJwtToken());
            request.Headers.TryAddWithoutValidation(":method", "POST");
            request.Headers.TryAddWithoutValidation(":path", path);
            request.Headers.Add("apns-topic", this.config.AppBundleIdentifier);
            //request.Headers.Add("apns-expiration", apnsExpiration.ToString());
            //request.Headers.Add("apns-priority", apnsPriority.ToString());

            var bg = notification.Aps.ContentAvailable == 1 && notification.Aps.Alert == null;
            request.Headers.Add("apns-push-type", bg ? "background" : "alert"); // for iOS 13 required

            //if (!string.IsNullOrWhiteSpace(apnsId))
            //{
            //    request.Headers.Add("apns-id", apnsId);
            //}

            //using (var response = await http.SendAsync(request, cancellationToken))
            //{
            //    var succeed = response.IsSuccessStatusCode;
            //    var content = await response.Content.ReadAsStringAsync();
            //    var error = JsonHelper.Deserialize<ApnsError>(content);

            //    return new ApnsResponse
            //    {
            //        IsSuccess = succeed,
            //        Error = error
            //    };
            //}
        }
    }
}
/*
 
        private static readonly ConcurrentDictionary<string, Tuple<string, DateTime>> tokens = new ConcurrentDictionary<string, Tuple<string, DateTime>>();
        private const int tokenExpiresMinutes = 50;


        private string GetJwtToken()
        {
            var (token, date) = tokens.GetOrAdd(settings.AppBundleIdentifier, _ => new Tuple<string, DateTime>(CreateJwtToken(), DateTime.UtcNow));
            if (date < DateTime.UtcNow.AddMinutes(-tokenExpiresMinutes))
            {
                tokens.TryRemove(settings.AppBundleIdentifier, out _);
                return GetJwtToken();
            }

            return token;
        }

        private string CreateJwtToken()
        {
            var header = JsonHelper.Serialize(new { alg = "ES256", kid = CleanP8Key(settings.P8PrivateKeyId) });
            var payload = JsonHelper.Serialize(new { iss = settings.TeamId, iat = ToEpoch(DateTime.UtcNow) });
            var headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
            var payloadBasae64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
            var unsignedJwtData = $"{headerBase64}.{payloadBasae64}";
            var unsignedJwtBytes = Encoding.UTF8.GetBytes(unsignedJwtData);

            using (var dsa = AppleCryptoHelper.GetEllipticCurveAlgorithm(CleanP8Key(settings.P8PrivateKey)))
            {
                var signature = dsa.SignData(unsignedJwtBytes, 0, unsignedJwtBytes.Length, HashAlgorithmName.SHA256);
                return $"{unsignedJwtData}.{Convert.ToBase64String(signature)}";
            }
        }

        private static int ToEpoch(DateTime time)
        {
            var span = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Convert.ToInt32(span.TotalSeconds);
        }

        private static string CleanP8Key(string p8Key)
        {
            // If we have an empty p8Key, then don't bother doing any tasks.
            if (string.IsNullOrEmpty(p8Key))
            {
                return p8Key;
            }

            var lines = p8Key.Split(new [] { '\n' }).ToList();

            if (0 != lines.Count && lines[0].StartsWith("-----BEGIN PRIVATE KEY-----"))
            {
                lines.RemoveAt(0);
            }

            if (0 != lines.Count && lines[lines.Count - 1].StartsWith("-----END PRIVATE KEY-----"))
            {
                lines.RemoveAt(lines.Count - 1);
            }

            var result = string.Join(string.Empty, lines);

            return result;
        }
    }
}


using Org.BouncyCastle.Crypto.Parameters;	
using Org.BouncyCastle.Security;
using System;
using System.Security.Cryptography;

namespace CorePush.Utils
{
    public static class AppleCryptoHelper
    {
        public static ECDsa GetEllipticCurveAlgorithm(string privateKey)	
        {	
            var keyParams = (ECPrivateKeyParameters) PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));	
            var q = keyParams.Parameters.G.Multiply(keyParams.D).Normalize();	

            return ECDsa.Create(new ECParameters	
            {	
                Curve = ECCurve.CreateFromValue(keyParams.PublicKeyParamSet.Id),	
                D = keyParams.D.ToByteArrayUnsigned(),	
                Q =	
                {	
                    X = q.XCoord.GetEncoded(),	
                    Y = q.YCoord.GetEncoded()	
                }	
            });	
        }
    }
}
 */