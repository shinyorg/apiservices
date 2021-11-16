﻿namespace Shiny.Api.Push.Management.Ef.Infrastructure
{
    public class DbNotificationRegistration
    {
        public Guid Id { get; set; }
        public PushPlatform Platform { get; set; }
        public string DeviceToken { get; set; }
        public string? UserId { get; set; }
        public DateTimeOffset? DateExpiry { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public ICollection<DbNotificationRegistrationTag> Tags { get; set; }
    }
}
