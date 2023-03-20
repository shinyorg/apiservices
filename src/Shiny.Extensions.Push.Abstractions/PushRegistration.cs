namespace Shiny.Extensions.Push;


public record PushRegistration(
    string Platform,
    string DeviceToken,
    string[]? Tags = null,
    string? UserId = null
);