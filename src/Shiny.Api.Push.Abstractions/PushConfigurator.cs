using Microsoft.Extensions.DependencyInjection;
using Shiny.Api.Push.Infrastructure;
using Shiny.Api.Push.Providers;


namespace Shiny.Api.Push
{
    public class PushConfigurator
    {
        public PushConfigurator(IServiceCollection services) => this.Services = services;


        public PushConfigurator UseRepository<TRepository>() where TRepository : class, IRepository
        {
            this.Services.AddScoped<IRepository, TRepository>();
            return this;
        }


        public IServiceCollection Services { get; }
        //public PushConfigurator AddDecorator(IAppleNotificationDecorator decorator)
        //{
        //    this.services.AddScoped<IAppleNotificationDecorator>(decorator);
        //    return this;
        //}

        //public PushConfigurator AddDecorator(IGoogleNotificationDecorator decorator)
        //{
        //    return this;
        //}

        public PushConfigurator AddApple(AppleConfiguration configuration)
        {
            return this;
        }


        public PushConfigurator AddGoogle(GoogleConfiguration configuration)
        {
            return this;
        }


        public PushConfigurator AddApple<TImpl>(GoogleConfiguration configuration) where TImpl : IApplePushProvider
        {
            return this;
        }

        public PushConfigurator AddGoogle<TImpl>(GoogleConfiguration configuration) where TImpl : IGooglePushProvider
        {
            return this;
        }
    }
}
