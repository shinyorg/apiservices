using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shiny.Extensions.Push.Providers;


namespace Shiny.Extensions.Push
{
    public class Notification
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? CategoryOrChannel { get; set; }
        public string? ImageUri { get; set; }
        public TimeSpan? Expiration { get; set; }
        public Dictionary<string, string>? Data { get; set; }

        public Func<PushRegistration, GoogleNotification, Task>? DecorateGoogle { get; set; }
        public Func<PushRegistration, AppleNotification, Task>? DecorateApple { get; set; }
    }
}
