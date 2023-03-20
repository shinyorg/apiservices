using Shiny.Extensions.Push.GoogleFirebase;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shiny.Extensions.Push.GoogleFirebase.Infrastructure;


public static class Serializer
{
    static readonly PushJsonSerializerContext JsonContext = new(new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.Default,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    });

    public static string Serialize(GoogleNotification notification)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(notification, JsonContext.GoogleNotification);
        return Encoding.UTF8.GetString(bytes);
    }

    public static FcmResponse? DeserialzeFcmResponse(string content)
        => JsonSerializer.Deserialize(content, JsonContext.FcmResponse);
}


[JsonSerializable(typeof(GoogleNotification))]
[JsonSerializable(typeof(FcmResponse))]
[JsonSerializable(typeof(FcmResult))]
[JsonSerializable(typeof(GoogleAndroidConfig))]
[JsonSerializable(typeof(GoogleAndroidNotificationDetails))]
[JsonSerializable(typeof(string))]
public partial class PushJsonSerializerContext : JsonSerializerContext
{
}
