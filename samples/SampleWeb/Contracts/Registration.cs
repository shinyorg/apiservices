using Shiny.Api.Push;

namespace SampleWeb.Contracts
{
    public class Registration
    {
        public string DeviceToken { get; set; }
        public string? UserId { get; set; }
        public PushPlatform Platform { get; set; }
        public string[]? Tags { get; set; }
    }
}
