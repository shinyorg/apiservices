namespace Shiny.Extensions.Push.Infrastructure;


public interface IPushProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    bool CanPushTo(PushRegistration registration);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="regiration"></param>
    /// <param name="notification"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task Send(INotification notification, PushRegistration regiration, CancellationToken cancelToken);
}