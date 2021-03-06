using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class AdoNetTemplateLoader<TDbConnection> : ITemplateLoader where TDbConnection : DbConnection, new()
    {
        readonly string connectionString;
        readonly string parameterPrefix;


        public AdoNetTemplateLoader(string connectionString, string parameterPrefix)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.parameterPrefix = parameterPrefix ?? "@";
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
            using var conn = new TDbConnection();
            using var command = conn.CreateCommand();

            conn.ConnectionString = this.connectionString;
            command.Parameters.Add(this.CreateParameter(command, "TemplateName", templateName));
            
            if (cultureCode == null)
            {
                command.CommandText = this.parameterPrefix == "@" ? SQL_DEFAULT : SQL_DEFAULT.Replace("@", this.parameterPrefix);
            }
            else
            {
                command.CommandText = this.parameterPrefix == "@" ? SQL : SQL.Replace("@", this.parameterPrefix);
                command.Parameters.Add(this.CreateParameter(command, "CultureCode", cultureCode));
            }
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


        static readonly string SQL = $"SELECT Content FROM MailTemplates WHERE TemplateName = @TemplateName AND CultureCode = @CultureCode";
        static readonly string SQL_DEFAULT = $"SELECT Content FROM MailTemplates WHERE TemplateName = @TemplateName AND CultureCodeIS NULL";
    }
}
