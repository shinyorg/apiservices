using Shiny.Extensions.Push.GoogleFirebase;

namespace Shiny.Extensions.Push;


public interface IGoogleEvents
{
    /// <summary>
    /// Allows you to customize the notification for the before it is sent
    /// </summary>
    /// <param name="registration"></param>
    /// <param name="notification"></param>
    /// <param name="native"></param>
    /// <returns></returns>
    Task OnBeforeSend(INotification notification, PushRegistration registration, GoogleNotification native);
}