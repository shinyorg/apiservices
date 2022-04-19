namespace Shiny.Extensions.Webhooks
{
    public interface IWebHookManager
    {
        Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName);
        Task Subscribe(WebHookRegistration registration);
        Task UnSubscribe(Guid registrationId);

        Task<T?> Request<T>(WebHookRegistration registration, object? args, CancellationToken cancelToken = default);
        Task Send(string eventName, object? args, CancellationToken cancelToken = default);
    }
}
