namespace Shiny.Extensions.Push.Infrastructure;

using Shiny.Extensions.Push.Providers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


internal static class Serializer
{
    public static string Serialize(AppleNotification notification)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(notification, PushJsonSerializerContext.Default.AppleNotification);
        return Encoding.UTF8.GetString(bytes);
    }


    public static string Serialize(GoogleNotification notification)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(notification, PushJsonSerializerContext.Default.GoogleNotification);
        return Encoding.UTF8.GetString(bytes);
    }

    public static FcmResponse? DeserialzeFcmResponse(string content)
        => JsonSerializer.Deserialize(content, PushJsonSerializerContext.Default.FcmResponse);
}


//https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
[JsonSerializable(typeof(AppleNotification), GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(GoogleNotification), GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(FcmResponse), GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(FcmResult), GenerationMode = JsonSourceGenerationMode.Serialization)]
internal partial class PushJsonSerializerContext : JsonSerializerContext
{
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
