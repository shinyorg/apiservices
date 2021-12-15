using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;


namespace Shiny.Push.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void UsePushApi(this IServiceCollection services, string registerUrl, string unregisterUrl, HttpClient? httpClient = null)
        {
            //services.AddSingleton<IPushManager, PushManager>();
        }
    }
}
