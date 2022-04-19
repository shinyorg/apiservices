namespace Shiny.Extensions.Webhooks
{
    public class DbRepositoryConfig
    {
        public DbRepositoryConfig(string connectionString)
            => this.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));


        public string ConnectionString { get; }
        public string ParameterPrefix { get; set; } = "@";
        public string TableName { get; set; } = "WebHookRegistrations";
        public bool InsertResults { get; set; } = true;
    }
}
