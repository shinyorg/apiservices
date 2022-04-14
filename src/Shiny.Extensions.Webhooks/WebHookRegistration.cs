namespace Shiny.Extensions.Webhooks;


public record WebHookRegistration(
    string EventName,
    string CallbackUri,
    string HashVerification,
    int ExecutionTimeoutSeconds = 30
);
