using Shiny.Extensions.Push.Apple;

namespace Shiny.Extensions.Push;


public interface IAppleNotificationCustomizer : INotification
{
    Func<AppleNotification, PushRegistration, Task>? AppleBeforeSend { get; set; }
}