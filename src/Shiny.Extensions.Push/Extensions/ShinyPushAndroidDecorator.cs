namespace Shiny.Extensions.Push.Extensions;

using Shiny.Extensions.Push.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;


public class ShinyPushAndroidDecorator : IGoogleNotificationDecorator
{
    public Task Decorate(PushRegistration registration, Notification notification, GoogleNotification nativeNotification, CancellationToken cancelToken)
    {
        nativeNotification.Android ??= new();
        nativeNotification.Android.Notification ??= new();
        if (String.IsNullOrWhiteSpace(nativeNotification.Android.Notification.ClickAction))
            nativeNotification.Android.Notification.ClickAction = "SHINY_NOTIFICATION_CLICK";

        return Task.CompletedTask;
    }
}
