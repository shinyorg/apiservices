# Managing Users

The push notification abstractions at their core are a minor part of this library.  The main "features" are the ones that manage not only the push tokens, but also the user tagging and general tags that don't come out of the box for all push providers like Apple.

## Register Users

```csharp
IPushManager pushManager; // inject this
await this.pushManager.Send(new PushRegistration {
    Platform = PushPlatform.Apple, // or Google
    DeviceToken = token, // the device token from the push provider on the mobile side
    UserId = "Your User's ID (optional)",
    Tags = new [] { "tag1", "tag2" } // tags (optional)
});

```

## Unregister All Users

```csharp
IPushManager pushManager; // inject this

await this.pushManager.UnRegister(PushPlatform.Apple, "DeviceToken");

// OR

await this.pushManager.UnRegisterByUser("UserId"); // unregistered a userId from all platforms & devices
```

## Sending a Push

Sending allows you to not only provide a fully set notification, but also provide a rich set of criteria for who to send to

```csharp
IPushManager pushManager; // inject this

await this.pushManager.Send(
    new Shiny.Extensions.Push.Notification
    {
        Title = "Your Notification Title",
        Message = "Your Message",
        Data = new Dictionary<string, string> {
            { "custom", "data" }
        }
    },

    // all arguments are optional here
    new PushFilter
    {
        DeviceToken = "DeviceToken", // if set, will only send to a specific device
        UserId = "Your UserId", // if set, will only send to a specific user
        Tags = new [] { "tag1" }, // if set, will only send to users with these tags
        Platform = PushPlatforms.Apple // if set, will only send to users on this platform
    }
);
```


## Querying
This is the exact same criteria as used by send, but obviously, without the sending :)

```csharp
IPushManager pushManager; // inject this

var registrations = await this.pushManager.GetRegistrations(    new PushFilter
{
    DeviceToken = "DeviceToken", // if set, will only retrieve to a specific device
    UserId = "Your UserId", // if set, will only retrieve to a specific user
    Tags = new [] { "tag1" }, // if set, will only retrieve to users with these tags
    Platform = PushPlatforms.Apple // if set, will only retrieve users on this platform
});
```