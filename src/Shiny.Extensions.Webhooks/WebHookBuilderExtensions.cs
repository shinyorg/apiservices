using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Webhooks.Infrastructure;
using System.Data.Common;


namespace Shiny.Extensions.Webhooks
{
    public static class WebHookBuilderExtensions
    {
        /// <summary>
        /// Add web hook management to your dependency injection container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="build"></param>
        public static void AddWebHookManagement(this IServiceCollection services, Action<WebHookBuilder>? build = null)
        {
            var builder = new WebHookBuilder(services);
            build?.Invoke(builder);
            builder.TrySetDefaults();
        }


        /// <summary>
        /// Registers a database webhook repository for use with your webhook manager
        /// </summary>
        /// <typeparam name="TDbConnectionType">The type of ADO.NET database connection to use</typeparam>
        /// <param name="services"></param>
        /// <param name="repositoryConfig"></param>
        /// <returns></returns>
        public static WebHookBuilder UseWebHookDbRepository<TDbConnectionType>(this WebHookBuilder builder, DbRepositoryConfig repositoryConfig)
            where TDbConnectionType : DbConnection, new()
            => builder.UseRepository(_ => new DbRepository<TDbConnectionType>(repositoryConfig));


        /// <summary>
        /// Registers a database webhook repository for use with your webhook manager
        /// </summary>
        /// <param name="services"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IServiceCollection UseWebHookFileRepository(this IServiceCollection services, string filePath)
            => services.AddScoped<IRepository>(_ => new FileRepository(filePath));
    }
}
