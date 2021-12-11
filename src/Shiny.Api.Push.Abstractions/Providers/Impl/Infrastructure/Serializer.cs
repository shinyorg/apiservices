using Shiny.Api.Push.Providers.Infrastructure;
using System.Text;
using System.Text.Json;


namespace Shiny.Api.Push.Providers.Impl.Infrastructure
{
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
    }
}
