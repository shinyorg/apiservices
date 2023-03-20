using Shiny.Extensions.Push.GoogleFirebase;

namespace Shiny.Extensions.Push;


public interface IGoogleNotificationCustomizer : INotification
{
    /// <summary>
    /// Allows you to customize a specific notification based on a push registration - this fires after all IGoogleEvents have fired
    /// </summary>
    Func<GoogleNotification, PushRegistration, Task>? GoogleBeforeSend { get; set; }
}

