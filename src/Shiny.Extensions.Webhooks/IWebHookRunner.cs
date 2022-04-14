namespace Shiny.Extensions.Webhooks;

public interface IWebHookRunner
{
    Task<T> RequestWithResponse<T>(WebHookRegistration registration, object args, CancellationToken cancelToken = CancellationToken.None);
    Task Send(string eventName, object args);
}
