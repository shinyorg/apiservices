namespace Shiny.Extensions.Localization.Tests.Databases;

using System;
using System.Data.Common;


public abstract class AdoNetProviderTests<TDbConnection> where TDbConnection: DbConnection, new()
{
    protected string? ConnectionString { get; set; }


    protected virtual int ExecuteNonQuery(string sql, params (string Name, object Value)[] parameters)
    {
        using (var conn = new TDbConnection())
        {
            conn.ConnectionString = this.ConnectionString;
            using (var command = conn.CreateCommand())
            {
                command.CommandText = sql;
                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.Name;
                    p.Value = parameter.Value ?? DBNull.Value;

                    command.Parameters.Add(p);
                }
                conn.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
