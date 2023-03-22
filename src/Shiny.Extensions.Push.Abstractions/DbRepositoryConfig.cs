namespace Shiny.Extensions.Push;

public record DbRepositoryConfig(
    string ConnectionString,
    string ParameterPrefix = "@",
    string TableName = "PushRegistrations",
    bool CreateTablesIfNotExist = false
);