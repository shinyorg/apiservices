using System;
using System.Collections.Generic;

namespace Shiny.Api.Push.Management.Models
{
    public class NotificationRegistrationModel
    {
        public int Id { get; set; }
        public string DeviceToken { get; set; }
        public string UserId { get; set; }
        public PushPlatform Platform { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public ICollection<NotificationRegistrationTag> Tags { get; set; }
    }
}
