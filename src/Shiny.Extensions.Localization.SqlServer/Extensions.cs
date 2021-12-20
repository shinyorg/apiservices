using Shiny.Extensions.Localization.SqlServer;


namespace Shiny.Extensions.Localization
{
    public static class Extensions
    {
        public static LocalizationBuilder AddSqlServer(this LocalizationBuilder builder, string connectionString)
            => builder.Add(new SqlServerLocalizationProvider(connectionString));
    }
}
