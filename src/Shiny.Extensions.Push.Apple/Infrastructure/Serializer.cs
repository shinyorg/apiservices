using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shiny.Extensions.Push.Apple.Infrastructure;


public static class Serializer
{
    static readonly PushJsonSerializerContext JsonContext = new(new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.Default,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    });


    public static string Serialize(AppleNotification notification)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(notification, JsonContext.AppleNotification);
        return Encoding.UTF8.GetString(bytes);
    }
}


//https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
[JsonSerializable(typeof(AppleNotification))]
[JsonSerializable(typeof(Aps))]
[JsonSerializable(typeof(ApsAlert))]
[JsonSerializable(typeof(string))]
public partial class PushJsonSerializerContext : JsonSerializerContext
{
}
