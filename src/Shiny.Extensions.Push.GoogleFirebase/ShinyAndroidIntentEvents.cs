namespace Shiny.Extensions.Push.GoogleFirebase;


public class ShinyAndroidIntentEvents : IGoogleEvents
{
    public static string AndroidClickAction { get; set; } = "SHINY_NOTIFICATION_CLICK";

    public Task OnBeforeSend(INotification notification, PushRegistration registration, GoogleNotification native)
    {
        native.Android!.Notification!.ClickAction = AndroidClickAction;
        return Task.CompletedTask;
    }
}