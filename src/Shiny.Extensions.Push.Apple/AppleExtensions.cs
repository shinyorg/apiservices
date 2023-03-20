using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Push.Apple.Infrastructure;
using Shiny.Extensions.Push.Infrastructure;

namespace Shiny.Extensions.Push;


public static class AppleExtensions
{
    public static PushConfigurator AddEvents<TEvents>(this PushConfigurator config) where TEvents : class, IAppleEvents
    {
        config.Services.AddSingleton<IAppleEvents, TEvents>();
        return config;
    }


    public static PushConfigurator AddApple(this PushConfigurator config, AppleConfiguration pushConfig)
    {
        pushConfig.AssertValid();

        config.Services.AddSingleton(pushConfig);
        config.Services.AddSingleton<IPushProvider, ApplePushProvider>();
        return config;
    }
}