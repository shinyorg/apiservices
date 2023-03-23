using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using RazorEngine.Templating;
using System.Xml.Linq;


namespace Shiny.Extensions.Mail.Impl
{
    public class AdoNetTemplateLoader<TDbConnection> : ITemplateLoader where TDbConnection : DbConnection, new()
    {
        readonly string connectionString;
        readonly string parameterPrefix;
        readonly string tableName;
        readonly bool tryCreateTables;


        public AdoNetTemplateLoader(
            string connectionString,
            string parameterPrefix,
            string tableName,
            bool tryCreateTables
        )
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.tableName = tableName;
            this.parameterPrefix = parameterPrefix ?? "@";
            this.tryCreateTables = tryCreateTables;
        }


        public async Task<string> Load(string templateName, CultureInfo? culture = null, CancellationToken cancellationToken = default)
        {
            if (culture == null)
                return await this.GetDefaultTemplate(templateName, cancellationToken);

            var template = await this.TryLoad(templateName, culture.Name, cancellationToken).ConfigureAwait(false);
            if (template == null)
            {
                template = await this.TryLoad(templateName, culture.TwoLetterISOLanguageName, cancellationToken).ConfigureAwait(false);
                if (template == null)
                    template = await this.GetDefaultTemplate(templateName, cancellationToken).ConfigureAwait(false);
            }
            return template!;
        }



        protected virtual async Task<string> GetDefaultTemplate(string templateName, CancellationToken cancellationToken)
        {
            var template = await this.TryLoad(templateName, null, cancellationToken).ConfigureAwait(false);
            if (template == null)
                throw new ArgumentException($"Template '{template}' does not exist");

            return template;
        }


        protected async Task<string?> TryLoad(string templateName, string? cultureCode, CancellationToken cancellationToken)
        {
            await this.TryCreateTables().ConfigureAwait(false);

            using var conn = new TDbConnection();
            using var command = conn.CreateCommand();

            conn.ConnectionString = this.connectionString;
            command.Parameters.Add(this.CreateParameter(command, "TemplateName", templateName));

            var sql = cultureCode == null ? SQL_DEFAULT : SQL;
            if (this.parameterPrefix != "@")
                sql = sql.Replace("@", this.parameterPrefix);                

            if (!this.tableName.Equals("MailTemplates"))
                sql = sql.Replace("MailTemplates", this.tableName);

            command.CommandText = sql;
            if (cultureCode != null)
                command.Parameters.Add(this.CreateParameter(command, "CultureCode", cultureCode));

            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            using var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken).ConfigureAwait(false);
            return reader.Read() ? reader.GetString(0) : null;
        }


        protected virtual DbParameter CreateParameter(DbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = this.parameterPrefix + name;
            parameter.Value = value?? DBNull.Value;

            return parameter;
        }


        bool tablesCreated = false;
        protected virtual async Task TryCreateTables()
        {
            if (!this.tryCreateTables || tablesCreated)
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
                    using var conn = new TDbConnection();
                    using var command = conn.CreateCommand();

                    conn.ConnectionString = this.connectionString;

                    if (!this.tableName.Equals("MailTemplates"))
                        sql = sql.Replace("MailTemplates", this.tableName);

                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch
                {
                    // catch & release
                }
            }
            tablesCreated = true;
        }


        const string SQLITE = $$"""
            CREATE TABLE IF NOT EXISTS MailTemplates(
                TemplateName TEXT NOT NULL,
                CultureCode TEXT NOT NULL,
                Content TEXT,
                PRIMARY KEY (TemplateName, CultureCode)
            );
            """;

        const string SQLSERVER = $$"""
            IF NOT EXISTS(SELECT* FROM sysobjects WHERE name = 'MailTemplates' and xtype = 'U')
                CREATE TABLE[dbo].[MailTemplates] (
                    [TemplateName][nvarchar](255) NOT NULL PRIMARY KEY,
                    [CultureCode] [varchar] (5) NULL PRIMARY KEY,
                    [Content][nvarchar] (max) NOT NULL
                )
            GO
            """;


        const string POSTGRES = $$"""
            CREATE TABLE IF NOT EXISTS MailTemplates(
                TemplateName varchar(255) NOT NULL,
                CultureCode varchar(5) NULL,
                Content varchar(2000) NULL,
                PRIMARY KEY(TemplateName, CultureCode)
            );
            """;

        static readonly string SQL = $"SELECT Content FROM TABLE_NAME WHERE TemplateName = @TemplateName AND CultureCode = @CultureCode";
        static readonly string SQL_DEFAULT = $"SELECT Content FROM TABLE_NAME WHERE TemplateName = @TemplateName AND CultureCodeIS NULL";
    }
}
