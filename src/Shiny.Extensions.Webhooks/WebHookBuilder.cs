using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Webhooks.Infrastructure;


namespace Shiny.Extensions.Webhooks
{
    public class WebHookBuilder
    {
        public WebHookBuilder(IServiceCollection services) => this.Services = services;
        public IServiceCollection Services { get; }


        public virtual WebHookBuilder ConfigureRunner(WebHookRunnerConfig config)
        {
            this.Services.AddSingleton(config);
            return this;
        }


        public virtual WebHookBuilder UseRunner<T>() where T : class, IRunner
        {
            this.Services.AddSingleton<IRunner, T>();
            return this;
        }


        public virtual WebHookBuilder UseRepository<T>() where T : class, IRepository
        {
            this.Services.AddScoped<IRepository, T>();
            return this;
        }


        public virtual WebHookBuilder UseRepository<T>(Func<IServiceProvider, T> activator) where T : class, IRepository
        {
            this.Services.AddScoped<IRepository>(sp => activator(sp));
            return this;
        }


        public virtual WebHookBuilder TrySetDefaults()
        {
            this.Services.TryAddSingleton<WebHookRunnerConfig>(_ => new());
            this.Services.TryAddSingleton<IRunner, Runner>();
            this.Services.TryAddScoped<IWebHookManager, WebHookManager>();
            this.Services.TryAddScoped<IRepository>(_ => new FileRepository("webhooks.json"));
            return this;
        }
    }
}
