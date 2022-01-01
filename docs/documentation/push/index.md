# Push Notification Management

|Name|Nuget|
|----|-----|
|Push|[!NugetShield(Shiny.Extensions.Push)]|
|EF Repository|[!NugetShield(Shiny.Extensions.Push.Ef)]|

There are tons of push management systems out there - Azure Notification Hubs, Amazon SQS, OneSignal, Firebase, etc, but what if you want to run this stuff local?  That also exists - PushSharp and others exist in the .NET ecosystem.

Currently, this library supports Apple Notifications and Google Firebase Messaging.  It is important you understand the native API's before using this library
    * [Firebase Cloud Messaging Documentation](https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages)
    * [Apple Notification Documentation](https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification)

However, there is a bunch of things that they don't offer
1. General purpose push notification abilities
2. Management of the registration base (tokens, which user owns the token, etc)
3. From the sender base, they lack a lot of deep customization
4. What if I just want to hit the base OS providers without the middle man?  

Shiny.Extensions.Push looks to solve all of these issues.  

```csharp
services.AddPushManagement(x => x
    .AddApplePush(new AppleConfiguration {
        IsProduction = true, // prod or sandbox
        TeamId = "Your Team ID",
        AppBundleIdentifier = "com.yourcompany.yourapp",
        Key = "Your Key With NO new lines from Apple Dev Portal",
        KeyId = "The KeyID for your cert"
    })
    .AddGooglePush(new GoogleConfiguration {
        SenderId = "Your Firebase Sender ID",
        ServerKey = "Your Firebase Server Key",
        DefaultChannelId = "The Default Channel to use on Android",
        UseShinyAndroidPushIntent = true // this is for Shiny.Push.X v2.5+ if you use it on Xamarin Mobile apps
    })
    .UseEfRepository<SampleDbContext>()
);
```

## Register a Device/User

Now that we've registered our push management components, we can assume they we will be injecting Shiny.Extensions.Push.IPushManager wherever we need it.

```csharp
using Shiny.Extensions.Push;


public class ExampleController : ControllerBase {
    readonly IPushManager pushManager;

    
    public ExampleController(IPushManager pushManager) {
        this.pushManager = pushManager;
    }


    public async Task Register(bool isApple, string token) {
        var userId = this.User.GetUserId(); // you can tag the device registration with the user id
        await this.pushManager.Send(new PushRegistration {
            Platform = PushPlatform.Apple,
            DeviceToken = token,
            UserId = userId.ToString(),
            Tags = new [] { "tag1", "tag2" }
        });
    }
}
```

## Sending a Push

Sending a push cross platform, couldn't be easier (or more powerful)

```csharp
await this.pushManager.Send(
    new Shiny.Extensions.Push.Notification
    {
        Title = "Hello",
        Message = "This is not spam really!",
        Data = new Dictionary<string, string> {
            { "custom", "data" }
        }
    },
    // what group of users you want to target
    new PushFilter
    {
        //DeviceToken = reg.DeviceToken,
        //UserId = reg.UserId,
        //Tags = reg.Tags,
        Platform = PushPlatforms.All
    }    
);
```