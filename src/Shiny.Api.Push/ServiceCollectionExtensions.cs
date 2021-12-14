namespace Shiny.Api.Push;

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Api.Push.Infrastructure;
using Shiny.Api.Push.Providers;


public static class ServiceCollectionExtensions
{
    public static void AddPushManagement(this IServiceCollection services, Action<PushConfigurator> configure)
    {
        var cfg = new PushConfigurator(services);
        configure(cfg);

        if (!services.Any(x => x.ServiceType == typeof(IPushManager)))
            services.AddTransient<IPushManager, PushManager>();
    }


    public static PushConfigurator AddApplePush(this PushConfigurator pushConfig, AppleConfiguration configuration)
    {
        pushConfig.Services.AddTransient<IApplePushProvider>(x => new ApplePushProvider(configuration));
        return pushConfig;
    }


    public static PushConfigurator AddGoogle(this PushConfigurator pushConfig, GoogleConfiguration configuration)
    {
        pushConfig.Services.AddTransient<IGooglePushProvider>(x => new GooglePushProvider(configuration));
        return pushConfig;
    }
}
