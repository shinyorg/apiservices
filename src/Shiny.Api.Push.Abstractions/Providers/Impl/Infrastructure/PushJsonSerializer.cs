using System.Text.Json;
using System.Text.Json.Serialization;


//https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
namespace Shiny.Api.Push.Providers.Infrastructure
{
    [JsonSerializable(typeof(AppleNotification))]
    [JsonSerializable(typeof(GoogleNotification), GenerationMode = JsonSourceGenerationMode.Serialization)]
    internal partial class PushJsonSerializer : JsonSerializerContext
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