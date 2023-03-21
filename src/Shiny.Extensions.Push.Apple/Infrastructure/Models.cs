using System;
using System.Text.Json.Serialization;

namespace Shiny.Extensions.Push.Apple;


public record JwtToken(string Value, DateTime Expires);

public class ApnResponse
{
    public string? Reason { get; set; }
    public long? Timestamp { get; set; }
}

public class ApnException : Exception
{
    public ApnException(string reason) : base("Apple returned invalid reason: " + reason) { }
}


public enum PushType
{
    Alert,
    Background,
    Voip,
    Complication,
    FileProvider,
    Mdm,
    Location,
    LiveActivity
}


/// <summary>
/// https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification
/// </summary>
public class AppleNotification
{
    [JsonIgnore]
    public string? ApnsId { get; set; }

    [JsonIgnore]
    public string? ApnsCollapseId { get; set; }

    [JsonIgnore]
    public int? ApnsPriority { get; set; }

    [JsonIgnore]
    public long? ExpirationFromEpoch { get; set; }

    [JsonIgnore]
    public PushType? PushType { get; set; }

    [JsonIgnore]
    public string? ApnsTopic { get; set; }

    [JsonPropertyName("aps")]
    public Aps? Aps { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object> CustomData { get; set; } = new();
}

public class Aps
{
    /// <summary>
    /// The information for displaying an alert. A dictionary is recommended. If you specify a string, the alert displays your string as the body text
    /// </summary>
    [JsonPropertyName("alert")]
    public ApsAlert? Alert { get; set; }

    /// <summary>
    /// The number to display in a badge on your app’s icon. Specify 0 to remove the current badge, if any
    /// </summary>
    [JsonPropertyName("badge")]
    public int? Badge { get; set; }

    /// <summary>
    /// The notification service app extension flag.If the value is 1, the system passes the notification to your notification service app extension before delivery.Use your extension to modify the notification’s content
    /// </summary>
    [JsonPropertyName("mutable-content")]
    public int? MutableContent { get; set; }

    /// <summary>
    /// A dictionary that contains sound information for critical alerts. For regular notifications, use the sound string instead.
    /// </summary>
    [JsonPropertyName("sound")]
    public string? Sound { get; set; } // TODO: string or dictionary for critical

    /// <summary>
    /// The background notification flag. To perform a silent background update, specify the value 1 and don’t include the alert, badge, or sound keys in your payload
    /// </summary>
    [JsonPropertyName("content-available")]
    public int? ContentAvailable { get; set; }

    /// <summary>
    /// The identifier of the window brought forward.The value of this key will be populated on the UNNotificationContent object created from the push payload. Access the value using the UNNotificationContent object’s targetContentIdentifier property
    /// </summary>
    [JsonPropertyName("target-content-id")]
    public string? TargetContentId { get; set; }

    /// <summary>
    /// The relevance score, a number between 0 and 1, that the system uses to sort the notifications from your app.The highest score gets featured in the notification summar
    /// </summary>
    [JsonPropertyName("relevance-score")]
    public double? RelevanceScore { get; set; }

    /// <summary>
    /// The notification’s type.This string must correspond to the identifier of one of the UNNotificationCategory objects you register at launch time
    /// </summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <summary>
    /// An app-specific identifier for grouping related notifications. This value corresponds to the threadIdentifier property in the UNNotificationContent object
    /// </summary>
    [JsonPropertyName("thread-id")]
    public string? ThreadId { get; set; }

    /// <summary>
    /// A string that indicates the importance and delivery timing of a notification. The string values “passive”, “active”, “time-sensitive”, or “critical” correspond to the UNNotificationInterruptionLevel enumeration cases
    /// </summary>
    [JsonPropertyName("interruption-level")]
    public string? InterruptionLevel { get; set; }


    //        Table 3 lists the keys that you may include in the sound dictionary.Use these keys to configure the sound for a critical alert.

    //Table 3 Keys to include in the sound dictionary
    //Key

    //Value type

    //Description

    //critical

    //Number

    //The critical alert flag.Set to 1 to enable the critical alert.

    //name

    //String

    //The name of a sound file in your app’s main bundle or in the Library/Sounds folder of your app’s container directory.Specify the string “default” to play the system sound.For information about how to prepare sounds, see UNNotificationSound.

    //volume


    //Number

    //The volume for the critical alert’s sound. Set this to a value between 0 (silent) and 1 (full volume).
}


public class ApsAlert
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("title-loc-key")]
    public string? TitleLocKey { get; set; }

    [JsonPropertyName("title-loc-args")]
    public string[]? TitleLocArgs { get; set; }

    //[JsonPropertyName("action-loc-key")]
    //public string? ActionLocKey { get; set; }

    //loc-key
    //loc-args[]

    [JsonPropertyName("subtitle")]
    public string? Subtitle { get; set; }

    [JsonPropertyName("subtitle-loc-key")]
    public string? SubtitleLocKey { get; set; }

    [JsonPropertyName("subtitle-loc-args")]
    public string[]? SubtitleLocArgs { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonPropertyName("launch-image")]
    public string? LaunchImage { get; set; }
}