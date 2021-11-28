namespace Shiny.Api.Push
{
    public class PushFilter
    {
        public string? DeviceToken { get; set; }
        public string? UserId { get; set; }
        public PushPlatforms Platform { get; set; } = PushPlatforms.All;
        public string[]? Tags { get; set; }
    }
}
