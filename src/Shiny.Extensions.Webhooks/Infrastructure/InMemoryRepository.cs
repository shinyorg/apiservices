namespace Shiny.Extensions.Webhooks.Infrastructure;

public class InMemoryRepository : IRepository
{
    public List<WebHookRegistration> Registrations { get; } = new();


    public Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName)
        => Task.FromResult(this.Registrations.Where(x => x.EventName.Equals(eventName, StringComparison.InvariantCultureIgnoreCase)));


    public Task Subscribe(WebHookRegistration registration)
    {
        this.Registrations.Add(registration);
        return Task.CompletedTask;
    }


    public Task UnSubscribe(Guid registrationId)
    {
        var reg = this.Registrations.FirstOrDefault(x => x.Id == registrationId);
        if (reg != null)
            this.Registrations.Remove(reg);

        return Task.CompletedTask;
    }
}
