namespace Shiny.Api.Push
{
    public class PushFilter
    {
        public string? DeviceToken { get; set; }
        public string? UserId { get; set; }
        public PushPlatform? Platform { get; set; }
        public string[] Tags { get; set; }
    }
}
