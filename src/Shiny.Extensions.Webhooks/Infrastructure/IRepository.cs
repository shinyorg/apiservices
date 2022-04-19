namespace Shiny.Extensions.Webhooks.Infrastructure;

public interface IRepository
{
    Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName);
    Task Subscribe(WebHookRegistration registration);
    Task UnSubscribe(Guid registrationId);
}
