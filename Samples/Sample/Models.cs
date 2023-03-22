using Shiny.Extensions.Push;

namespace Sample;

public record PushSendRequest(
    string NotificationTitle,
    string NotificationMessage,
    Filter? Filter = null
);