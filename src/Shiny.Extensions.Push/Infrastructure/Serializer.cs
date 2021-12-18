using Shiny.Extensions.Push.Providers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Shiny.Extensions.Push.Infrastructure
{
    internal static class Serializer
    {
        static readonly PushJsonSerializerContext JsonContext = new(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });


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
    [JsonSerializable(typeof(GoogleNotification))]
    [JsonSerializable(typeof(FcmResponse))]
    [JsonSerializable(typeof(FcmResult))]
    [JsonSerializable(typeof(GoogleAndroidConfig))]
    [JsonSerializable(typeof(GoogleAndroidNotificationDetails))]
    [JsonSerializable(typeof(ApnResponse))]
    internal partial class PushJsonSerializerContext : JsonSerializerContext
    {
    }
}
//Person person = new() { FirstName = "Jane", LastName = "Doe" };
//byte[] utf8Json = JsonSerializer.SerializeToUtf8Bytes(person, MyJsonContext.Default.Person);
//person = JsonSerializer.Deserialize(utf8Json, MyJsonContext.Default.Person):


//JsonSerializerOptions options = new()
//{
//    ReferenceHander = ReferenceHandler.Preserve,
//    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//};

//// Use your custom options to initialize a context instance.
//MyJsonContext context = new(options);

//string json = JsonSerializer.Serialize(person, context.Person);
