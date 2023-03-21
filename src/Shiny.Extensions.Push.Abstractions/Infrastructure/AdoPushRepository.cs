using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Extensions.Push.Infrastructure;


public class AdoPushRepository<TDbConnection> : IPushRepository
    where TDbConnection : DbConnection, new()
{
    readonly DbRepositoryConfig config;
    public AdoPushRepository(DbRepositoryConfig config)
        => this.config = config;


    public async Task<IList<PushRegistration>> Get(Filter? filter, CancellationToken cancelToken)
    {
        var list = new List<PushRegistration>();
        var wc = BuildWhereClause(filter);

        await this.Do(
            async command =>
            {
                var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    // TODO: tags
                    // TODO: null safety
                    list.Add(new PushRegistration
                    (
                        reader.GetString(0),
                        reader.GetString(1),
                        Tags: new[] { "" },
                        UserId: ""
                    ));
                }
            },
            $"SELECT DeviceToken, Platform, UserId FROM {this.config.TableName}" + wc?.WhereClause,
            wc?.Parameters!
        );

        return list;
    }


    public Task Remove(Filter filter, CancellationToken cancelToken)
    {
        var wc = BuildWhereClause(filter);
        if (wc == null)
            throw new InvalidOperationException("You must provide minimum 1 filter to REMOVE");

        return this.Do(
            conn => conn.ExecuteNonQueryAsync(cancelToken),
            "DELETE FROM PushRegistrations" + wc.Value.WhereClause,
            wc.Value.Parameters!
        );
    }


    public Task RemoveBatch(PushRegistration[] pushRegistrations, CancellationToken cancelToken)
    {
        throw new NotImplementedException();
    }


    public Task Save(PushRegistration reg, CancellationToken cancelToken)
    {
        // TODO: upsert, tags
        return this.Do(
            conn => conn.ExecuteNonQueryAsync(cancelToken),
            "INSERT INTO (Platform, DeviceToken, UserId) VALUES (@p1, @p2, @p3)",
            ("p1", reg.Platform),
            ("p2", reg.DeviceToken),
            ("p3", reg.UserId)
            // TODO: merge tags with ;
        );
    }


    static (string WhereClause, (string ParameterName, object ParameterValue)[] Parameters)? BuildWhereClause(Filter? filter)
    {
        if (filter == null)
            return null;

        var ps = new List<(string ParameterName, object ParameterValue)>();
        var wc = " WHERE ";

        if (!String.IsNullOrWhiteSpace(filter.UserId))
        {
            wc = "UserId = @w1";
            ps.Add(("w1", filter.UserId));
        }

        if (!String.IsNullOrWhiteSpace(filter.DeviceToken))
        {
            if (ps.Count > 1) wc += " AND ";
            wc += "DeviceToken = @w2";
            ps.Add(("w2", filter.DeviceToken));
        }

        //if (filter.Platform != PushPlatforms.All)
        //{
        //    if (ps.Count > 1) wc += " AND ";
        //    wc += "Platform = @w3";
        //    ps.Add(("w3", filter.Platform));
        //}

        // TODO: tags
        return (wc, ps.ToArray());
    }


    async Task Do(Func<DbCommand, Task> doWork, string sql, params (string ParameterName, object? Value)[] args)
    {
        using var connection = new TDbConnection();

        var prefix = this.config.ParameterPrefix;
        if (prefix != "@")
            sql = sql.Replace("@", prefix);

        connection.ConnectionString = this.config.ConnectionString;
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


    async Task CreateTables()
    {
        var ns = typeof(TDbConnection).Namespace;

        //"Microsoft.Data.Sqlite"
        //"Microsoft.Data.SqlClient"
    }
}