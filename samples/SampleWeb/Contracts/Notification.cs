using Shiny.Api.Push;

namespace SampleWeb.Contracts
{
    public class Notification
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string>? Data { get; set; }

        public string[] Tags { get; set; }
        public string? UserId { get; set; }
        public string? DeviceToken { get; set; }
        public PushPlatform? Platform { get; set; }
    }
}
