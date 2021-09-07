using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Api.Push.Providers;


namespace Shiny.Api.Push
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplePush(this IServiceCollection services)
        {
            services.TryAddSingleton<IPushNotificationManager, PushNotificationManager>();
            services.AddSingleton<IPushProvider, ApplePushProvider>();
        }


        public static void AddGooglePush(this IServiceCollection services)
        {
            services.TryAddSingleton<IPushNotificationManager, PushNotificationManager>();
            services.AddSingleton<IPushProvider, GooglePushProvider>();
        }


        public static void AddWindowsPush(this IServiceCollection services)
        {
            services.TryAddSingleton<IPushNotificationManager, PushNotificationManager>();
            services.AddSingleton<IPushProvider, WindowsNotificationPushProvider>();
        }
    }
}
