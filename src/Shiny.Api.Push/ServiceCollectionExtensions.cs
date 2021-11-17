using System;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Api.Push.Infrastructure;

namespace Shiny.Api.Push
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPushManagement(this IServiceCollection services, Action<PushConfigurator> configure)
        {
            services.AddSingleton<IPushManager, PushManager>();

        }
    }
}
