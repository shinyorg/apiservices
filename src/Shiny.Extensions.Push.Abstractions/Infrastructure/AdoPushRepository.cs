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


    public virtual async Task<IList<PushRegistration>> Get(Filter? filter, CancellationToken cancelToken)
    {
        var list = new List<PushRegistration>();
        var wc = this.BuildWhereClause(filter);

        await this.CreateTables().ConfigureAwait(false);
        await this
            .Do(
                async command =>
                {
                    var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        list.Add(new PushRegistration
                        (
                            reader.GetString(0),
                            reader.GetString(1),                            
                            reader.IsDBNull(2) ? null : reader.GetString(2),
                            reader.IsDBNull(3) ? null : reader.GetString(3).Split(';')
                        ));
                    }
                },
                $"SELECT Platform, DeviceToken, UserId, Tags FROM {this.config.TableName}{wc?.WhereClause}",
                wc?.Parameters!
            )
            .ConfigureAwait(false);

        return list;
    }


    public virtual async Task Remove(Filter filter, CancellationToken cancelToken)
    {
        var wc = this.BuildWhereClause(filter);
        if (wc == null)
            throw new InvalidOperationException("You must provide minimum 1 filter to REMOVE");

        await this.CreateTables().ConfigureAwait(false);
        await this
            .Do(
                conn => conn.ExecuteNonQueryAsync(cancelToken),
                $"DELETE FROM {this.config.TableName} {wc.Value.WhereClause}",
                wc.Value.Parameters!
            )
            .ConfigureAwait(false);
    }


    public virtual async Task RemoveBatch(PushRegistration[] pushRegistrations, CancellationToken cancelToken)
    {
        if (pushRegistrations.Length == 0)
            throw new InvalidOperationException("No registrations to remove");

        await this.CreateTables().ConfigureAwait(false);

        var parms = new List<(string ParameterName, object? Value)>();
        var sql = $"DELETE FROM {this.config.TableName} WHERE ";

        for (var i = 0; i < pushRegistrations.Length; i++)
        {
            if (i > 0) sql += " OR ";

            var platform = $"p{i}_p";
            var deviceToken = $"p{i}_dt";
            var reg = pushRegistrations[i];

            sql += $"(Platform = @{platform} AND DeviceToken = @{deviceToken})";
            parms.Add((platform, reg.Platform));
            parms.Add((deviceToken, reg.DeviceToken));
        }

        await this
            .Do(
                conn => conn.ExecuteNonQueryAsync(cancelToken),
                sql,
                parms.ToArray()
            )
            .ConfigureAwait(false);
    }


    public async Task Save(PushRegistration reg, CancellationToken cancelToken)
    {
        await this.CreateTables().ConfigureAwait(false);

        string? tags = null;
        if (reg.Tags?.Any() ?? false)
        {
            tags = String.Join(";", reg.Tags) + ";"; // add end of parameter for like query
        }
        await this
            .Do(
                conn => conn.ExecuteNonQueryAsync(cancelToken),
                this.GetUpsert(),
                ("p1", reg.Platform),
                ("p2", reg.DeviceToken),
                ("p3", reg.UserId),
                ("p4", tags)
            )
            .ConfigureAwait(false);
    }


    protected virtual string GetUpsert()
    {
        if (typeof(TDbConnection).Namespace == "Microsoft.Data.SqlClient")
        {
            return $$"""
                MERGE TABLE_NAME AS [Target]
                USING (SELECT Platform = @p1, DeviceToken = @p2) AS [Source] 
                    ON [Target].Platform = [Source].Platform AND [Target].DeviceToken = [Source].DeviceToken
                WHEN MATCHED THEN
                    UPDATE SET [Target].UserId = @p3, [Target].Tags = @p4
                WHEN NOT MATCHED THEN
                    INSERT (Platform, DeviceToken, UserId, Tags) VALUES (@p1, @p2, @p3, @p4);
                """.Replace("TABLE_NAME", this.config.TableName);
        }
        return $"INSERT INTO {this.config.TableName}(Platform, DeviceToken, UserId, Tags) VALUES (@p1, @p2, @p3, @p4) ON CONFLICT(Platform, DeviceToken) DO UPDATE SET UserId = @p3, Tags = @p4";
    }


    protected virtual (string WhereClause, (string ParameterName, object ParameterValue)[] Parameters)? BuildWhereClause(Filter? filter)
    {
        if (filter == null)
            return null;

        var ps = new List<(string ParameterName, object ParameterValue)>();
        var wc = "";

        if (!String.IsNullOrWhiteSpace(filter.UserId))
        {
            wc += "UserId = @w1";
            ps.Add(("w1", filter.UserId));
        }

        if (!String.IsNullOrWhiteSpace(filter.DeviceToken))
        {
            if (ps.Count > 0) wc += " AND ";
            wc += "DeviceToken = @w2";
            ps.Add(("w2", filter.DeviceToken));
        }

        // TODO: verify platform filter if not null
        if (!String.IsNullOrWhiteSpace(filter.Platform))
        {
            if (ps.Count > 0) wc += " AND ";
            wc += "Platform = @w3";
            ps.Add(("w3", filter.Platform));
        }

        if (filter.Tags?.Any() ?? false)
        {
            if (ps.Count > 0) wc += " AND ";
            for (var i = 0; i < filter.Tags.Length; i++)
            {
                if (i > 0) wc += " AND ";
                wc += $"Tags LIKE @w4_{i}";

                // ensure semi-colon to check split
                ps.Add(("w4_" + i, $"%{filter.Tags[i]};%"));
            }
        }
        if (ps.Count > 0)
            wc = " WHERE " + wc;

        return (wc, ps.ToArray());
    }


    protected async Task Do(Func<DbCommand, Task> doWork, string sql, params (string ParameterName, object? Value)[] args)
    {
        using var connection = new TDbConnection();

        var prefix = this.config.ParameterPrefix;
        if (prefix != "@")
            sql = sql.Replace("@", prefix);

        connection.ConnectionString = this.config.ConnectionString;
        using var command = connection.CreateCommand();
        command.CommandText = sql;
#if DEBUG
        Console.WriteLine("SQL: " + sql);
#endif

        foreach (var arg in args)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = prefix + arg.ParameterName;
            parameter.Value = arg.Value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        await connection.OpenAsync().ConfigureAwait(false);
        await doWork(command).ConfigureAwait(false);
    }


    bool tablesCreated = false;
    protected virtual async Task CreateTables()
    {
        if (!this.config.CreateTablesIfNotExist || tablesCreated)
            return;

        var sql = typeof(TDbConnection).Namespace switch
        {
            "Microsoft.Data.Sqlite" => SQLITE,
            "Microsoft.Data.SqlClient" => SQLSERVER,
            "Npgsql" => POSTGRES,
            _ => null
        };
        if (sql != null)
        {
            try
            {
                sql = sql.Replace("TABLE_NAME", this.config.TableName);
                await this.Do(x =>
                    x.ExecuteNonQueryAsync(),
                    sql
                );
            }
            catch
            {
                // catch & release
            }
        }
        tablesCreated = true;
    }

    const string SQLITE = $$"""
        CREATE TABLE IF NOT EXISTS TABLE_NAME (
            Platform    TEXT NOT NULL,
            DeviceToken TEXT NOT NULL,
            UserId      TEXT,
            Tags        TEXT,
            PRIMARY KEY (Platform, DeviceToken)
        );
        """;

    const string SQLSERVER = $$"""
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'TABLE_NAME' and xtype = 'U')
            CREATE TABLE [dbo].[TABLE_NAME](
                [Platform] [varchar](10) NOT NULL PRIMARY KEY,
                [DeviceToken] [varchar](50) NOT NULL PRIMARY KEY,
                [UserId] [varchar](50) NULL,
                [Tags] [varchar](2000) NULL
            )
        GO
        """;


    const string POSTGRES = $$"""
        CREATE TABLE IF NOT EXISTS TABLE_NAME (
            Platform varchar(10) NOT NULL,
            DeviceToken varchar(50) NOT NULL,
            UserId varchar(50) NULL,
            Tags varchar(2000) NULL,
            PRIMARY KEY(Platform, DeviceToken)
        );
        """;
}