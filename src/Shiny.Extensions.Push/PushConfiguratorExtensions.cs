using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Push.Extensions;
using Shiny.Extensions.Push.Infrastructure;
using Shiny.Extensions.Push.Providers;


namespace Shiny.Extensions.Push
{
    public static class PushConfiguratorExtensions
    {
        public static void AddPushManagement(this IServiceCollection services, Action<PushConfigurator> configure)
        {
            var cfg = new PushConfigurator(services);
            configure(cfg);

            services.TryAddSingleton<IAppleAuthTokenProvider, AppleAuthTokenProvider>();
            services.TryAddScoped<IPushManager, PushManager>();
        }


        public static PushConfigurator AddShinyPushAndroidDecorator(this PushConfigurator pushConfig)
            => pushConfig.AddGoogleDecorator<ShinyPushAndroidDecorator>();


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
            ArgumentNullException.ThrowIfNull(configuration);
            configuration.AssertValid();
            pushConfig.Services.AddSingleton<IAppleConfigurationProvider>(new ConfiguredAppleConfigurationProvider(configuration));
            return pushConfig.AddApplePush();
        }


        public static PushConfigurator AddApplePush<TConfigProvider>(this PushConfigurator pushConfig)
            where TConfigProvider : class, IAppleConfigurationProvider
        {
            pushConfig.Services.AddScoped<IAppleConfigurationProvider, TConfigProvider>();
            return pushConfig;
        }


        public static PushConfigurator AddApplePush(this PushConfigurator pushConfig)
        {
            pushConfig.Services.TryAddScoped<IApplePushProvider, ApplePushProvider>();
            return pushConfig;
        }


        public static PushConfigurator AddGooglePush<TConfigProvider>(this PushConfigurator pushConfig)
            where TConfigProvider : class, IGoogleConfigurationProvider
        {
            pushConfig.Services.AddScoped<IGoogleConfigurationProvider, TConfigProvider>();
            pushConfig.Services.AddScoped<IGooglePushProvider, GooglePushProvider>();
            return pushConfig;
        }


        public static PushConfigurator AddGooglePush(this PushConfigurator pushConfig, GoogleConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            configuration.AssertValid();

            pushConfig.Services.AddSingleton<IGoogleConfigurationProvider>(new ConfiguredGoogleConfigurationProvider(configuration));
            return pushConfig.AddGooglePush();
        }


        public static PushConfigurator AddGooglePush(this PushConfigurator pushConfig)
        {
            pushConfig.Services.TryAddScoped<IGooglePushProvider, GooglePushProvider>();
            return pushConfig;
        }


        public static PushConfigurator AddGoogleConfigurationProvider<TConfigProvider>(this PushConfigurator pushConfig) where TConfigProvider : class, IGoogleConfigurationProvider
        {
            pushConfig.Services.AddScoped<IGoogleConfigurationProvider, TConfigProvider>();
            return pushConfig;
        }
    }
}
