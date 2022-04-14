using System.Data.Common;

namespace Shiny.Extensions.Webhooks.Impl;

public class DbWebHookRepository<TDbConnection>
    where TDbConnection : DbConnection,
    IWebHookRepository
{
    readonly string connectionString;
    public DbWebHookRepository(string connectionString) => this.connectionString = connectionString;


    public Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName)
    {
        throw new NotImplementedException();
    }

    public Task Save(WebHookRegistration registration)
    {
        throw new NotImplementedException();
    }

    public Task SaveResult(WebHookRegistration registration, bool success, string? result)
    {
        throw new NotImplementedException();
    }
}
