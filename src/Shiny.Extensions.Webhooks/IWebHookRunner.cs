namespace Shiny.Extensions.Webhooks;

public interface IWebHookRunner
{
    Task<T> RequestWithResponse<T>(WebHookRegistration registration, object args, CancellationToken cancelToken = default);
    Task Send(string eventName, object args);
}
