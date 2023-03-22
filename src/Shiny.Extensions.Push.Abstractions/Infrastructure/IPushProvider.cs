namespace Shiny.Extensions.Push.Infrastructure;


public interface IPushProvider
{
    /// <summary>
    /// Checks if a pushregistration can be used through this provider
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    bool CanPushTo(PushRegistration reg);

    /// <summary>
    /// Checks if a platform is supported
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    bool CanPushTo(string platform);

    /// <summary>
    /// Sends a notification through to a specific registration
    /// </summary>
    /// <param name="regiration"></param>
    /// <param name="notification"></param>
    /// <param name="cancelToken"></param>
    /// <returns>True if provider says notification sent, false if it detects something like an invalid device token</returns>
    Task<bool> Send(INotification notification, PushRegistration registration, CancellationToken cancelToken);
}