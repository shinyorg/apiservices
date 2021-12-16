namespace Shiny.Extensions.Push.Providers;

using System.Collections.Generic;
using System.Text.Json.Serialization;


public class AppleNotification : Dictionary<string, string>
{
    public Aps Aps { get; set; }
    //    CustomData = null,
    //    Headers = null
}

public class Aps
{
    public ApsAlert? Alert { get; set; }
    public int? Badge { get; set; }

    [JsonPropertyName("mutable-content")]
    public int? MututableContent { get; set; }
    public string? Sound { get; set; }

    [JsonPropertyName("content-available")]
    public int? ContentAvailable { get; set; }
    public string? Category { get; set; }
    public string? ThreadId { get; set; }
    //    CustomData = null,
}


public class ApsAlert
{
    public string? Title { get; set;}
    //public string TitleLocKey { get; set; }
    //public string[] TitleLocArgs { get; set; }

    //[JsonPropertyName("action-loc-key")]
    //public string? ActionLocKey { get; set; }

    //loc-key
    //loc-args[]

    public string? Subtitle { get; set; }
    //public string SubtitleLocKey { get; set; }
    //public string[] SubtitleLocArgs { get; set; }

    public string? Body { get; set; }
    public string? LaunchImage { get; set; }
}