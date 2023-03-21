namespace Shiny.Extensions.Push.Infrastructure;


public interface IPushProvider
{
    /// <summary>
    /// Checks if a pushregistration can be used through this provider
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    bool CanPushTo(PushRegistration registration);

    /// <summary>
    /// Sends a notification through to a specific registration
    /// </summary>
    /// <param name="regiration"></param>
    /// <param name="notification"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task Send(INotification notification, PushRegistration registration, CancellationToken cancelToken);
}