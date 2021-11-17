using System;
using System.Collections.Generic;


namespace Shiny.Api.Push
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? CategoryOrChannel { get; set; }
        public string? ImageUri { get; set; }
        public IDictionary<string, string> Data { get; set; }

        //public Action<GoogleNotification>? Decorate { get; set; }
        //public Action<AppleNotification>? Decorate { get; set; }
    }
}
