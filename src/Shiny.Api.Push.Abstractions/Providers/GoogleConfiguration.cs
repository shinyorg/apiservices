﻿namespace Shiny.Api.Push.Providers
{
    public class GoogleConfiguration
    {
        public string SenderId { get; init; }
        public string ServerKey { get; init; }
        public string? DefaultChannelId { get; set; }
        public bool UseShinyAndroidPushIntent { get; set; } = true;
    }
}