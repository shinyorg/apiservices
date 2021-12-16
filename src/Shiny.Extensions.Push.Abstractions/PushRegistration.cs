using System;


namespace Shiny.Extensions.Push
{
    public class PushRegistration
    {
        public string? UserId { get; set; }
        public string DeviceToken { get; set; }
        public PushPlatforms Platform { get; set; }
        public string[]? Tags { get; set; }

        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
