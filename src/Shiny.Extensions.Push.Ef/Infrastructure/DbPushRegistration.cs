using System;
using System.Collections.Generic;


namespace Shiny.Extensions.Push.Ef
{
    public class DbPushRegistration
    {
        public Guid Id { get; set; }
        public PushPlatforms Platform { get; set; }
        public string DeviceToken { get; set; }
        public string? UserId { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public ICollection<DbPushTag> Tags { get; set; }
    }
}
