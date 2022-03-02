# Configuration Providers

Configuration providers were spawned out of the need for multitenancy without the need for mucking up our push providers.  For single tenant, you won't even
realize these things are there.

## Custom Provider

1. Implement the Shiny.Extensions.Push.Providers.IAppleConfigurationProvider and/or IGoogleConfigurationProvider

```csharp
namespace YourNamespace;

using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.Push;
using Shiny.Extensions.Push.Providers;
using System.Threading.Tasks;


public class PushConfigurationProvider : IAppleConfigurationProvider, IGoogleConfigurationProvider
{
    public async Task<GoogleConfiguration> GetConfiguration(Notification notification)
    {
        var cfg = await this.GetConfig(notification).ConfigureAwait(false);
        return new GoogleConfiguration
        {
            SenderId = cfg.GooglePushSenderId,
            ServerKey = cfg.GooglePushServerKey,
            UseShinyAndroidPushIntent = true
        };
    }


    async Task<AppleConfiguration> IAppleConfigurationProvider.GetConfiguration(Notification notification)
    {
        var cfg = await this.GetConfig(notification).ConfigureAwait(false);
        return new AppleConfiguration
        {
            TeamId = cfg.ApplePushTeamId,
            Key = cfg.ApplePushKey,
            KeyId = cfg.ApplePushKeyId,
            AppBundleIdentifier = cfg.ApplePushAppBundleIdentifier,
            IsProduction = cfg.ApplePushProduction
        };
    }

    async 
}
```

2. Register it with your DI container

```csharp
services.AddPushManagement(x => x
    .AddApplePush<PushConfigurationProvider>()
    .AddGooglePush<PushConfigurationProvider>()
);

```