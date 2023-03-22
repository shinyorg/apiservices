using System.Text.Json;

namespace Shiny.Extensions.Push.Infrastructure;


public class FilePushRepository : IPushRepository
{
    readonly FileInfo fileInfo;
    readonly List<PushRegistration> registrations;


    public FilePushRepository(string? filePath = null)
    {
        this.fileInfo = new FileInfo(filePath ?? "pushdb.json");
        if (!this.fileInfo.Exists)
        {
            File.Create(this.fileInfo.FullName);
        }
        else
        {
            var json = File.ReadAllText(this.fileInfo.FullName);
            if (!String.IsNullOrWhiteSpace(json))
                this.registrations = JsonSerializer.Deserialize<List<PushRegistration>>(json!)!;
        }
        this.registrations ??= new List<PushRegistration>();
    }


    Task Flush()
    {
        lock (this.fileInfo)
            File.WriteAllText(this.fileInfo.FullName, JsonSerializer.Serialize(this.registrations));

        return Task.CompletedTask;
    }


    public Task<IList<PushRegistration>> Get(Filter? filter, CancellationToken cancelToken)
        => Task.FromResult(this.GetRegs(filter));


    public Task Remove(Filter filter, CancellationToken cancelToken)
    {
        foreach (var reg in this.GetRegs(filter))
            this.registrations.Remove(reg);

        return this.Flush();
    }


    public Task RemoveBatch(PushRegistration[] pushRegistrations, CancellationToken cancelToken)
    {
        foreach (var delete in pushRegistrations)
        {
            var reg = this.registrations.FirstOrDefault(x =>
                x.DeviceToken == delete.DeviceToken &&
                x.Platform == x.Platform
            );
            if (reg != null)
                this.registrations.Remove(reg);
        }
        return this.Flush();
    }


    public Task Save(PushRegistration reg, CancellationToken cancelToken)
    {
        this.registrations.Add(reg);
        return this.Flush();
    }


    IList<PushRegistration> GetRegs(Filter? filter)
    {
        var query = this.registrations.AsQueryable();

        if (filter != null)
        {
            if (!String.IsNullOrWhiteSpace(filter.DeviceToken))
                query = query.Where(x => x.DeviceToken.Equals(filter.DeviceToken));

            if (!String.IsNullOrWhiteSpace(filter.UserId))
                query = query.Where(x => x.UserId == filter.UserId);

        }
        var result = this.registrations.ToList();
        return result;
    }
}

