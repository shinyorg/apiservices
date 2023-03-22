using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Push.Extensions;
using Shiny.Extensions.Push.Infrastructure;

namespace Shiny.Extensions.Push;


public static class PushConfiguratorExtensions
{
    public static IServiceCollection AddPushManagement(this IServiceCollection services, Action<PushConfigurator> configAction)
    {
        var cfg = new PushConfigurator(services);
        configAction(cfg);
        services.AddSingleton<IPushManager, PushManager>();

        if (services.Count(x => x.ServiceType == typeof(IPushRepository)) != 1)
            throw new InvalidOperationException("There must be 1 push repository registered");

        //if (!services.Any(x => x.ServiceType == typeof(IPushProvider)))
        //    throw new InvalidOperationException("There must be at least 1 push provider registered");

        return services;
    }


    public static PushConfigurator AddAutoRemoveNoReceive(this PushConfigurator pushConfig)
        => pushConfig.AddReporter<AutoCleanupNotificationReporter>();


    public static PushConfigurator AddPerformanceLogger(this PushConfigurator pushConfig)
        => pushConfig.AddReporter<BatchTimeNotificationReporter>();


    public static PushConfigurator AddReporter<TReporter>(this PushConfigurator config) where TReporter : class, INotificationReporter
    {
        config.Services.AddSingleton<INotificationReporter, TReporter>();
        return config;
    }


    public static PushConfigurator AddProvider<TProvider>(this PushConfigurator config) where TProvider : class, IPushProvider
    {
        config.Services.AddSingleton<IPushProvider, TProvider>();
        return config;
    }


    public static PushConfigurator UseRepository<TRepository>(this PushConfigurator config) where TRepository : class, IPushRepository
    {
        config.Services.AddSingleton<IPushRepository, TRepository>();
        return config;
    }


    public static PushConfigurator UseFileRepository(this PushConfigurator config, string? rootPath = null)
    {
        config.Services.AddSingleton<IPushRepository>(_ => new FilePushRepository(rootPath));
        return config;
    }


    public static PushConfigurator UseAdoNetRepository<TDbConnection>(this PushConfigurator config, DbRepositoryConfig repoConfig)
        where TDbConnection : DbConnection, new()
    {
        config.Services.AddSingleton<IPushRepository>(_ => new AdoPushRepository<TDbConnection>(repoConfig));
        return config;
    }
}