﻿using System;


namespace Shiny.Api.Push
{
    public class PushRegistration
    {
        public string? UserId { get; set; }
        public string DeviceToken { get; set; }
        public PushPlatforms Platform { get; set; }
        public string[]? Tags { get; set; }

        public DateTimeOffset? DateExpiry { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}