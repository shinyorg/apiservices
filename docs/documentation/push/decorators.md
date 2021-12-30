# Decorators

While our general push "Notification" object handles a fair bit in a nice cross platform manner, there are definitely going to be times that you need to do more with the native push objects.  

Enter "Decorators".  Decorators offer you a way to see the "ready to send" native message alongside the push registration info (user, tags, platform, etc) as well as the notification object you used to establish any additional context that you want to add to the native notification.

## Creating a Decorator

Below is an example of an Apple and a Google notification decorator.  Note that you can alter the native notification however you deem fit here.

```csharp
public class MyAppleDecorator : Shiny.Extensions.Push.IGoogleNotificationDecorator
{
    public async Task Decorate(PushRegistration registration, Notification notification, AppleNotification nativeNotification, CancellationToken cancelToken)
    {
        // you could also retrieve the user for this notification, but careful, this will slow down the batch
        nativeNotification.Aps.Alert.Body += "Hello " + registration.UserId;
    }
}


public class MyAppleDecorator : Shiny.Extensions.Push.IGoogleNotificationDecorator
{
    public async Task Decorate(PushRegistration registration, Notification notification, GoogleNotification nativeNotification, CancellationToken cancelToken)
    {
        // you could also retrieve the user for this notification, but careful, this will slow down the batch
        nativeNotification.Android.Notification.Body += "Hello " + registration.UserId;
    }
}
```

## Registering a Decorator

You must now register the decorator with Shiny's push extension.  IMPORTANT: all decorators, much like the rest of Shiny push is singleton!

```csharp
build.Services.AddPushManagement(x => x
    .AddApplePush(...)
    .AddGooglePush(...)
    .AddAppleDecorator<MyAppleDecorator>()
    .AddGoogleDecorator<MyGoogleDecorator>()
    .UseEfRepository<SampleDbContext>()
);
```

[!NOTE]
You can have multiple decorators per notification platform