namespace Shiny.Extensions.Push;


public record PushRegistration(
    string Platform,
    string DeviceToken,    
    string? UserId = null,
    string[]? Tags = null
);