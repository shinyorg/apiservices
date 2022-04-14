namespace Shiny.Extensions.Webhooks;

public interface IWebHookRepository
{
    Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName);
    Task Save(WebHookRegistration registration);

    Task SaveResult(WebHookRegistration registration, bool success, string? result);
}
