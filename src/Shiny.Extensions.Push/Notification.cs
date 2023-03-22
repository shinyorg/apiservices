using Shiny.Extensions.Push.Apple;
using Shiny.Extensions.Push.GoogleFirebase;

namespace Shiny.Extensions.Push;


public class Notification : INotification, IAppleNotificationCustomizer, IGoogleNotificationCustomizer
{ 
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? CategoryOrChannel { get; set; }
    public string? ImageUri { get; set; }
    public TimeSpan? Expiration { get; set; }
    public Dictionary<string, string>? Data { get; set; } = new();
    public Func<AppleNotification, PushRegistration, Task>? AppleBeforeSend { get; set; }
    public Func<GoogleNotification, PushRegistration, Task>? GoogleBeforeSend { get; set; }
}