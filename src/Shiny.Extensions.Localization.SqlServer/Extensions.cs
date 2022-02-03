using Shiny.Extensions.Localization.SqlServer;


namespace Shiny.Extensions.Localization
{
    public static class Extensions
    {
        /// <summary>
        /// Add the SQL Server provider to the localization builder as a localization source
        /// </summary>
        /// <param name="builder">Your current builder</param>
        /// <param name="connectionString">The SQL Server connection string</param>
        /// <param name="appFilter">This will add an additional filter (WHERE AppIdentifier = 'YOURVALUE') to the query</param>
        /// <returns></returns>
        public static LocalizationBuilder AddSqlServer(this LocalizationBuilder builder, string connectionString, string? appFilter = null)
            => builder.Add(new SqlServerLocalizationProvider(connectionString, appFilter));
    }
}
