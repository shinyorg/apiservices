using Shiny.Extensions.Push.Apple;

namespace Shiny.Extensions.Push;


public interface IAppleEvents
{
    /// <summary>
    /// Allows you to customize the apple notification after it has passed through default setup
    /// </summary>
    /// <param name="notification">The standard notification info</param>
    /// <param name="registration">The push registration/user that you may wish to customize for</param>
    /// <param name="native">The default built apple notification</param>
    /// <returns></returns>
    Task OnBeforeSend(INotification notification, PushRegistration registration, AppleNotification native);
}