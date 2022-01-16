namespace Shiny.Extensions.Push
{ 
    using Microsoft.Extensions.DependencyInjection;
    using Shiny.Extensions.Push.Infrastructure;
    using Shiny.Extensions.Push.Providers;


    public class PushConfigurator
    {
        public PushConfigurator(IServiceCollection services) => this.Services = services;

        public IServiceCollection Services { get; }


        public PushConfigurator UseRepository<TRepository>() where TRepository : class, IRepository
        {
            this.Services.AddScoped<IRepository, TRepository>();
            return this;
        }


        public PushConfigurator AddReporter<TReporter>() where TReporter : class, INotificationReporter
        {
            this.Services.AddScoped<INotificationReporter, TReporter>();
            return this;
        }


        #region Apple

        public PushConfigurator AddApplePush<TImpl>() where TImpl : class, IApplePushProvider
        {
            this.Services.AddScoped<IApplePushProvider, TImpl>();
            return this;
        }


        public PushConfigurator AddAppleDecorator<TImpl>() where TImpl : class, IAppleNotificationDecorator
        {
            this.Services.AddScoped<IAppleNotificationDecorator, TImpl>();
            return this;
        }

        #endregion

        #region Google

        public PushConfigurator AddGooglePush<TImpl>() where TImpl : class, IGooglePushProvider
        {
            this.Services.AddScoped<IGooglePushProvider, TImpl>();
            return this;
        }


        public PushConfigurator AddGoogleDecorator<TImpl>() where TImpl : class, IGoogleNotificationDecorator
        {
            this.Services.AddScoped<IGoogleNotificationDecorator, TImpl>();
            return this;
        }

        #endregion
    }
}