namespace Shiny.Extensions.Push;


public interface IPushManager
{
    Task<IList<PushRegistration>> GetRegistrations(Filter? filter, CancellationToken cancellationToken = default);
    Task Send(INotification notification, Filter? filter, CancellationToken cancellationToken = default);
    Task Send(INotification notification, PushRegistration[] registrations, CancellationToken cancellationToken = default);

    Task Register(PushRegistration registration);
    Task UnRegister(string platform, string deviceToken, CancellationToken cancelToken = default);
    Task UnRegister(string userId, CancellationToken cancelToken = default);
}