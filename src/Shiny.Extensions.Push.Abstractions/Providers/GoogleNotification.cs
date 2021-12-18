namespace Shiny.Extensions.Push.Providers
{ 
    using System.Collections.Generic;
    using System.Text.Json.Serialization;


    //https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages

    public class GoogleNotification
    {
        //public string Name { get; set; } - response messageid
        //public string Topic { get; set; }
        //public string Condition { get; set; }
        public string To { get; set; }
        public string Token { get; set; }
        public Dictionary<string, string>? Data { get; set; }
        public GoogleAndroidConfig? Android { get; set; }
    }


    public enum GoogleAndroidMessagePriority
    {
        Normal,
        High
    }


    public class GoogleAndroidConfig
    {
        [JsonPropertyName("ttl")]
        public string? TimeToLive { get; set; } // ie. "3.5s"

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GoogleAndroidMessagePriority Priority { get; set; } = GoogleAndroidMessagePriority.Normal;

        [JsonPropertyName("collapse_key")]
        public string? CollapseKey { get; set; }

        [JsonPropertyName("restricted_package_name")]
        public string? RestrictedPackageName { get; set; }

        [JsonPropertyName("direct_boot_ok")]
        public bool DirectBootOk { get; set; }

        public GoogleAndroidNotificationDetails? Notification { get; set; }
    }


    public class GoogleAndroidNotificationDetails
    {
        // TODO: there is a fair bit more to go into this thing
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Sound { get; set; }
        public string? Tag { get; set; }
        public string? ImageUrl { get; set; }

        [JsonPropertyName("click_action")]
        public string? ClickAction { get; set; }

        [JsonPropertyName("title_loc_key")]
        public string? TitleLocKey { get; set; }

        [JsonPropertyName("title_loc_args")]
        public string[]? TitleLocArgs { get; set; }

        [JsonPropertyName("body_loc_key")]
        public string? BodyLocKey { get; set; }

        [JsonPropertyName("body_loc_args")]
        public string[]? BodyLocArgs { get; set; }

        [JsonPropertyName("channel_id")]
        public string? ChannelId { get; set; }
    }
}