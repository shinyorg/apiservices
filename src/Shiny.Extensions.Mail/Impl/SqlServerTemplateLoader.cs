using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    // TODO: build in memory cache
    public class SqlServerTemplateLoader : ITemplateLoader
    {
        readonly string connectionString;
        public SqlServerTemplateLoader(string connectionString)
            => this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));


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
            var template = await this.TryLoad(templateName, DefaultCulture, cancellationToken).ConfigureAwait(false);
            if (template == null)
                throw new ArgumentException($"Template '{template}' does not exist");

            return template;
        }


        protected async Task<string?> TryLoad(string templateName, string cultureCode, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    command.Parameters.AddRange(new [] {
                        new SqlParameter("@TemplateName", templateName),
                        new SqlParameter("@CultureCode", cultureCode)
                    });

                    await conn.OpenAsync(cancellationToken).ConfigureAwait(false);

                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken).ConfigureAwait(false))
                    {
                        if (reader.Read())
                            reader.GetString(0);
                    }
                }
            }
            return null;
        }

        const string DefaultCulture = "DEFAULT";
        static readonly string SQL = $"SELECT Content FROM MailTemplates WHERE TemplateName = @TemplateName AND ISNULL(CultureCode, '{DefaultCulture}') = @CultureCode";
    }
}
