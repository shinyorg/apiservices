# Notification Reporters

Notifications are often sent in "batches" depending on how wide your notification search parameters are.  

Reporters offer you a way of:
* Tracking Errors on a specific notification to a device
* Tracking when the batch begins and how many devices are going to be sent for processing

## Creating a Reporter

First thing to do, is implement the Shiny Notification Reporter interface like so:

```csharp
public class MyCustomReporter : Shiny.Extensions.Push.INotificationReporter
{
    public Task OnBatchCompleted(Guid batchId, IReadOnlyCollection<PushRegistration> success, IReadOnlyCollection<(PushRegistration Registration, Exception Exception)> failures, Notification notification, CancellationToken cancelToken)
    {
        ...
    }

    public Task OnBatchStart(Guid batchId, IReadOnlyCollection<PushRegistration> registrations, Notification notification, CancellationToken cancelToken)
    {
        ...
    }

    public Task OnNotificationError(Guid batchId, PushRegistration registration, Notification notification, Exception exception, CancellationToken cancelToken)
    {
        ...
    }

    public Task OnNotificationSuccess(Guid batchId, PushRegistration registration, Notification notification, CancellationToken cancelToken)
    {
        ...
    }
}
```

> [!NOTE]
> You can also use the `Shiny.Extensions.Push.NotificationReporter` class and use override instead of implementing the entire interface.

> [!NOTE]
> You can have multiple reporters

Next thing to do is register this with the extension during startup

```csharp
builder.Services.AddPushManagement(x => x
    .AddApplePush(...)
    .AddGooglePush(...)
    .AddReporter<MyCustomReporter>()
);
```

> [!WARNING]
> The reporter is registered as a singleton

> [!WARNING]
> It isn't good to do a lot of logic inside notification error or notification success as they run per notifcation in a batch.  It is better to use OnBatchStart and OnBatchCompleted to process.  This will allow your notification batches to finish quicker.

## Out of the Box Reporters
* **BatchTimeNotificationReporter** - this will log how long a batch takes to process.  You can use this to tune your queries or even determine that you may need to move your push functions to a more resilient platform like Azure Functions instead of directly in your Web API.

* **AutoCleanupNotificationReporter** - This reporter will remove any notification errors that occur because no notification was sent.  This usually means the user uninstalled your app (therefore never unregistered using your app) or has unregistered from notifications but it never hit your server.  It happens.  This will catch the exceptions and unregister the device for you.  To Setup, use the following during the extension registration

```csharp