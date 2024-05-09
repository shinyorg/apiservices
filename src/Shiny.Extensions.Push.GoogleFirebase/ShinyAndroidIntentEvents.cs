namespace Shiny.Extensions.Push.GoogleFirebase;


public class ShinyAndroidIntentEvents : IGoogleEvents
{
    public static string AndroidClickAction { get; set; } = "SHINY_PUSH_NOTIFICATION_CLICK";

    public Task OnBeforeSend(INotification notification, PushRegistration registration, GoogleNotification native)
    {
        native.Android ??= new();
        native.Android.Notification ??= new();
        if (String.IsNullOrWhiteSpace(native.Android.Notification.ClickAction))
            native.Android.Notification.ClickAction = AndroidClickAction;

        return Task.CompletedTask;
    }
}