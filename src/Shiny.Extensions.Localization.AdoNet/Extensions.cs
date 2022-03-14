using Microsoft.Data.SqlClient;
using System.Data.Common;
using Shiny.Extensions.Localization.AdoNet;


namespace Shiny.Extensions.Localization
{
    public static class LocalizationExtensions
    {
        /// <summary>
        /// Add the SQL Server provider to the localization builder as a localization source
        /// </summary>
        /// <param name="builder">Your current builder</param>
        /// <param name="connectionString">The SQL Server connection string</param>
        /// <param name="appFilter">This will add an additional filter (WHERE AppIdentifier = 'YOURVALUE') to the query</param>
        /// <returns></returns>
        public static LocalizationBuilder AddSqlServer(this LocalizationBuilder builder, string connectionString, string? appFilter = null)
            => builder.Add(new AdoNetLocalizationProvider<SqlConnection>(connectionString, appFilter));


        /// <summary>
        /// Add a generic ADO.NET driver
        /// </summary>
        /// <typeparam name="TDbConnection"></typeparam>
        /// <param name="builder">Your current builder</param>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="appFilter">This will add an additional filter (WHERE AppIdentifier = 'YOURVALUE') to the query</param>
        /// <returns></returns>
        public static LocalizationBuilder AddAdoNet<TDbConnection>(this LocalizationBuilder builder, string connectionString, string? appFilter = null) where TDbConnection : DbConnection, new()
            => builder.Add(new AdoNetLocalizationProvider<TDbConnection>(connectionString, appFilter));
    }
}
