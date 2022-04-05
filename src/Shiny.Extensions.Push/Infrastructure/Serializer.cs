using Shiny.Extensions.Push.Providers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Shiny.Extensions.Push.Infrastructure
{
    public static class Serializer
    {
        static readonly PushJsonSerializerContext JsonContext = new(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Default,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        // need kebab naming strategy
        //static readonly PushJsonSerializerContext AppleJsonContext = new(new JsonSerializerOptions
        //{
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    PropertyNameCaseInsensitive = true,
        //    Encoder = JavaScriptEncoder.Default,
        //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //});

        public static string Serialize(AppleNotification notification)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(notification, JsonContext.AppleNotification);
            return Encoding.UTF8.GetString(bytes);
        }


        public static string Serialize(GoogleNotification notification)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(notification, JsonContext.GoogleNotification);
            return Encoding.UTF8.GetString(bytes);
        }

        public static FcmResponse? DeserialzeFcmResponse(string content)
            => JsonSerializer.Deserialize(content, JsonContext.FcmResponse);

        public static ApnResponse? DeserialzeAppleResponse(string content)
            => JsonSerializer.Deserialize(content, JsonContext.ApnResponse);
    }


    //https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
    [JsonSerializable(typeof(AppleNotification))]
    [JsonSerializable(typeof(Aps))]
    [JsonSerializable(typeof(ApsAlert))]
    [JsonSerializable(typeof(GoogleNotification))]
    [JsonSerializable(typeof(FcmResponse))]
    [JsonSerializable(typeof(FcmResult))]
    [JsonSerializable(typeof(GoogleAndroidConfig))]
    [JsonSerializable(typeof(GoogleAndroidNotificationDetails))]
    [JsonSerializable(typeof(ApnResponse))]
    [JsonSerializable(typeof(string))]
    public partial class PushJsonSerializerContext : JsonSerializerContext
    {
    }
}
