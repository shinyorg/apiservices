using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Push.GoogleFirebase;
using Shiny.Extensions.Push.GoogleFirebase.Infrastructure;
using Shiny.Extensions.Push.Infrastructure;

namespace Shiny.Extensions.Push;


public static class GoogleNotifications
{
    public static PushConfigurator AddEvents<TEvents>(this PushConfigurator config) where TEvents : class, IGoogleEvents
    {
        config.Services.AddSingleton<IGoogleEvents, TEvents>();
        return config;
    }


    public static PushConfigurator AddGoogleFirebase(this PushConfigurator config, GoogleConfiguration providerConfig)
    {
        providerConfig.AssertValid();
        config.Services.AddSingleton(providerConfig);
        config.Services.AddSingleton<IPushProvider, GooglePushProvider>();
        return config;
    }


    public static PushConfigurator AddShinyAndroidClickAction(this PushConfigurator config, string clickAction = "SHINY_NOTIFICATION_CLICK")
    {
        ShinyAndroidIntentEvents.AndroidClickAction = clickAction;
        return config.AddEvents<ShinyAndroidIntentEvents>();
    }
}