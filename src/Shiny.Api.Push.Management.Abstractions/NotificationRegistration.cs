using System;


namespace Shiny.Api.Push.Management
{
    public class NotificationRegistration
    {
        public string? UserId { get; set; }
        public string DeviceToken { get; set; }
        public PushPlatform Platform { get; set; }
        public string[]? Tags { get; set; }
        
        public DateTimeOffset? DateExpiry { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
