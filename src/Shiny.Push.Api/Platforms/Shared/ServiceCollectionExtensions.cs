using Microsoft.Extensions.DependencyInjection;
using Shiny.Push;


namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static bool UsePushApi(this IServiceCollection services, PushConfiguration configuration)
        {
#if NETSTANDARD
            return false;
#else
            services.AddSingleton(configuration);
            services.AddSingleton<IPushManager, PushManager>();
            return true;
#endif
        }
    }
}
