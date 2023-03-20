namespace Shiny.Extensions.Push;

public record DbRepositoryConfig(
    string ConnectionString,
    string ParameterPrefix = "@",
    string TableName = "PushRegistrations",
    string TagTableName = "PushRegistrationTags",
    bool CreateTablesIfNotExist = false
);