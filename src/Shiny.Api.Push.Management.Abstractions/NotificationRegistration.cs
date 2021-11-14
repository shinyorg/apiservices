using System;
using System.Collections.Generic;


namespace Shiny.Api.Push.Management
{
    public class NotificationRegistration
    {
        public string? UserId { get; set; }
        public string Token { get; set; }
        public PushPlatform Platform { get; set; }
        public IDictionary<string, string> Tags { get; set; }
        // unreg date?
        public DateTimeOffset? DateExpiry { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
