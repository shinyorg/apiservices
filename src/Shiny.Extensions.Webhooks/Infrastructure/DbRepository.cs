namespace Shiny.Extensions.Webhooks.Infrastructure;

using System.Data.Common;


public class DbRepository<TDbConnection> : IRepository
    where TDbConnection : DbConnection, new()
{
    readonly DbRepositoryConfig configuration;
    public DbRepository(DbRepositoryConfig configuration) => this.configuration = configuration;


    public async Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName)
    {
        var list = new List<WebHookRegistration>();
        await this.Do(
            async command =>
            {
                var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    list.Add(new WebHookRegistration(
                        reader.GetString(0),
                        reader.GetString(reader.GetOrdinal("")),
                        reader.GetString(reader.GetOrdinal("")),
                        reader.GetInt32(reader.GetOrdinal(""))
                    )
                    {
                        Id = reader.GetGuid(0)
                    });
                }
            },
            $"SELECT Id, EventName, CallbackUri, HashVerification, TimeoutSeconds FROM {this.configuration.RegistrationTableName} WHERE EventName = @EventName",
            ("EventName", eventName)
        );

        return list;
    }


    public Task Subscribe(WebHookRegistration registration) => this.Do(
        command => command.ExecuteNonQueryAsync(),
        $"INSERT INTO {this.configuration.RegistrationTableName}(WebHookRegistrationId, EventName, CallbackUri, HashVerification, TimeoutSeconds) VALUES (@Id, @EventName, @CallbackUri, @HashVerification, @TimeoutSeconds)",
        ("Id", registration.Id),
        ("EventName", registration.EventName),
        ("CallbackUri", registration.CallbackUri),
        ("HashVerification", registration.HashVerification),
        ("ExecutionTimeoutSeconds", registration.ExecutionTimeoutSeconds)
    );


    public Task UnSubscribe(Guid registrationId) => this.Do(
        command => command.ExecuteNonQueryAsync(),
        $"DELETE FROM {this.configuration.RegistrationTableName} WHERE WebHookRegistrationId = @Id",
        ("Id", registrationId)
    );


    public Task SaveResult(WebHookRegistration registration, bool success, string? result)
    {
        if (!this.configuration.InsertResults)
            return Task.CompletedTask;

        return this.Do(
            command => command.ExecuteNonQueryAsync(),
            "INSERT INTO WebHookResults(WebHookRegistrationId, Success, Result, Timestamp) VALUES (@Id, @Success, @Result, @Timestamp)",
            ("Id", registration.Id),
            ("Success", success),
            ("Result", result),
            ("Timestamp", DateTime.UtcNow)
        );
    }


    async Task Do(Func<DbCommand, Task> doWork, string sql, params (string ParameterName, object? Value)[] args)
    {
        using var connection = new TDbConnection();

        var prefix = this.configuration.ParameterPrefix;
        if (prefix != "@")
            sql = sql.Replace("@", prefix);

        connection.ConnectionString = this.configuration.ConnectionString;
        using var command = connection.CreateCommand();
        command.CommandText = sql;

        foreach (var arg in args)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = prefix + arg.ParameterName;
            parameter.Value = arg.Value ?? DBNull.Value;
            command.Parameters.Add(arg);
        }

        await connection.OpenAsync().ConfigureAwait(false);
        await doWork(command).ConfigureAwait(false);
    }
}
