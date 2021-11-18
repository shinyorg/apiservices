using System.Text.Json.Serialization;

namespace Shiny.Api.Push.Providers
{
    public class GoogleNotification
    {
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
