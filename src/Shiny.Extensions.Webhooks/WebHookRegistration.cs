namespace Shiny.Extensions.Webhooks;


public record WebHookRegistration(
    string EventName,
    string CallbackUri,
    string HashVerification,
    int? ExecutionTimeoutSeconds
)
{
    public Guid Id { get; set; } = Guid.NewGuid();
};
