namespace Shiny.Api.Push;

using Microsoft.Extensions.DependencyInjection;
using Shiny.Api.Push.Infrastructure;
using Shiny.Api.Push.Providers;


public class PushConfigurator
{
    public PushConfigurator(IServiceCollection services) => this.Services = services;


    public PushConfigurator UseRepository<TRepository>() where TRepository : class, IRepository
    {
        this.Services.AddTransient<IRepository, TRepository>();
        return this;
    }


    public IServiceCollection Services { get; }


    #region Apple

    public PushConfigurator AddApplePush<TImpl>() where TImpl : class, IApplePushProvider
    {
        this.Services.AddTransient<IApplePushProvider, TImpl>();
        return this;
    }


    public PushConfigurator AddAppleDecorator<TImpl>() where TImpl : class, IAppleNotificationDecorator
    {
        this.Services.AddTransient<IAppleNotificationDecorator, TImpl>();
        return this;
    }

    #endregion

    #region Google

    public PushConfigurator AddGooglePush<TImpl>() where TImpl : class, IGooglePushProvider
    {
        this.Services.AddTransient<IGooglePushProvider, TImpl>();
        return this;
    }

    public PushConfigurator AddGoogleDecorator<TImpl>() where TImpl : class, IGoogleNotificationDecorator
    {
        this.Services.AddTransient<IGoogleNotificationDecorator, TImpl>();
        return this;
    }

    #endregion
}