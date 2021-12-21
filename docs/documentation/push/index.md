# Push Notification Management

There are tons of push management systems out there - Azure Notification Hubs, Amazon SQS, OneSignal, Firebase, etc, but what if you want to run this stuff local?  That also exists - PushSharp and others exist in the .NET ecosystem.

However, there is a bunch of things that they don't offer
1. General purpose push notification abilities
2. Management of the registration base (tokens, which user owns the token, etc)
3. From the sender base, they lack a lot of deep customization
4. What if I just want to hit the base OS providers without the middle man?  

Shiny.Extensions.Push looks to solve all of these issues.  

```csharp
services.AddPushManagement(x => x
    .AddApplePush(cfg.GetSection("Push:Apple").Get<AppleConfiguration>())
    .AddGooglePush(cfg.GetSection("Push:Google").Get<GoogleConfiguration>())
    .UseEfRepository<SampleDbContext>()
);
```