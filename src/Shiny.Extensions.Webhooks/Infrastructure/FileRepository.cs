namespace Shiny.Extensions.Webhooks.Infrastructure;

using System.Text.Json;

public class FileRepository : IRepository
{
    readonly string filePath;


    public FileRepository(string filePath)
    {
        if (!File.Exists(filePath))
            File.Create(filePath);

        this.filePath = filePath;
    }


    public Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName)
        => Task.FromResult(this.Registrations.AsEnumerable());


    public Task Subscribe(WebHookRegistration registration)
    {
        // TODO: set/override
        this.Registrations.Add(registration);
        this.Flush();
        return Task.CompletedTask;
    }


    public Task UnSubscribe(Guid registrationId)
    {
        var reg = this.Registrations.FirstOrDefault(x => x.Id == registrationId);
        if (reg != null)
        {
            this.Registrations.Remove(reg);
            this.Flush();
        }
        return Task.CompletedTask;
    }


    protected virtual void Flush()
    {
        lock (this.syncLock)
        {
            var content = JsonSerializer.Serialize(this.registrations);
            File.WriteAllText(this.filePath, content);
        }
    }


    readonly object syncLock = new();
    List<WebHookRegistration>? registrations;
    protected virtual List<WebHookRegistration> Registrations
    {
        get
        {
            if (this.registrations == null)
            {
                lock (this.syncLock)
                {
                    if (this.registrations == null)
                    {
                        var contents = File.ReadAllText(this.filePath);
                        this.registrations = JsonSerializer.Deserialize<List<WebHookRegistration>>(contents) ?? new();
                    }
                }
            }
            return this.registrations!;
        }
    }
}
