namespace Shiny
{ 
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Shiny.Extensions.Push;
    using Shiny.Extensions.Push.Extensions;
    using Shiny.Extensions.Push.Infrastructure;
    using Shiny.Extensions.Push.Providers;


    public static class ServiceCollectionExtensions
    {
        public static void AddPushManagement(this IServiceCollection services, Action<PushConfigurator> configure)
        {
            var cfg = new PushConfigurator(services);
            configure(cfg);

            services.TryAddSingleton<IPushManager, PushManager>();
        }


        public static PushConfigurator AddAutoRemoveNoReceive(this PushConfigurator pushConfig)
            => pushConfig.AddReporter<AutoCleanupNotificationReporter>();

        public static PushConfigurator AddPerformanceLogger(this PushConfigurator pushConfig)
            => pushConfig.AddReporter<BatchTimeNotificationReporter>();


        public static PushConfigurator AddAppleConfigurationProvider<TConfigProvider>(this PushConfigurator pushConfig) where TConfigProvider : class, IAppleConfigurationProvider
        {
            pushConfig.Services.AddScoped<IAppleConfigurationProvider, TConfigProvider>();
            return pushConfig;
        }


        public static PushConfigurator AddApplePush(this PushConfigurator pushConfig, AppleConfiguration configuration)
        {
            pushConfig.Services.AddSingleton<IAppleConfigurationProvider>(new ConfiguredAppleConfigurationProvider(configuration));
            pushConfig.Services.AddScoped<IApplePushProvider, ApplePushProvider>();
            return pushConfig;
        }


        public static PushConfigurator AddGoogleConfigurationProvider<TConfigProvider>(this PushConfigurator pushConfig) where TConfigProvider : class, IGoogleConfigurationProvider
        {
            pushConfig.Services.AddScoped<IGoogleConfigurationProvider, TConfigProvider>();
            return pushConfig;
        }


        public static PushConfigurator AddGooglePush(this PushConfigurator pushConfig, GoogleConfiguration configuration)
        {
            pushConfig.Services.AddSingleton<IGoogleConfigurationProvider>(new ConfiguredGoogleConfigurationProvider(configuration));
            pushConfig.Services.AddScoped<IGooglePushProvider, GooglePushProvider>();
            return pushConfig;
        }
    }
}
