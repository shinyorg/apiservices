using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Shiny.Extensions.Push.Infrastructure;


public class PushManager : IPushManager
{
    readonly IPushRepository repository;
    readonly ILogger logger;
    readonly IEnumerable<IPushProvider> providers;
    readonly IEnumerable<INotificationReporter> reporters;


    public PushManager(
        //PushConfigurator config,
        IPushRepository repository,
        ILogger<PushManager> logger,
        IEnumerable<IPushProvider> providers,
        IEnumerable<INotificationReporter> reporters
    )
	{
        this.repository = repository;
        this.logger = logger;
        this.providers = providers;
        this.reporters = reporters;
	}


    public Task<IList<PushRegistration>> GetRegistrations(Filter? filter, CancellationToken cancellationToken = default)
        => this.repository.Get(filter, cancellationToken);


    public Task Register(PushRegistration registration)
    {
        if (!this.providers.Any(x => x.CanPushTo(registration)))
            throw new InvalidOperationException("Invalid platform - " + registration.Platform);

        return this.repository.Save(registration, CancellationToken.None);
    }


    public Task UnRegister(string platform, string deviceToken, CancellationToken cancelToken = default)
    {
        if (String.IsNullOrEmpty(platform))
            throw new ArgumentNullException(nameof(platform));

        if (String.IsNullOrEmpty(deviceToken))
            throw new ArgumentNullException(nameof(deviceToken));

        //if (!this.providers.Any(x => x.CanPushTo(platform)))
        //    throw new InvalidOperationException("Invalid platform - " + platform);

        return this.repository.Remove(
            new Filter
            {
                Platform = platform ,
                DeviceToken = deviceToken
            },
            cancelToken
        );
    }


    public Task UnRegister(string userId, CancellationToken cancelToken = default)
    {
        if (String.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        return this.repository.Remove(new Filter { UserId = userId }, cancelToken);
    }


    public async Task Send(INotification notification, Filter? filter, CancellationToken cancellationToken = default)
    {
        notification = notification ?? throw new ArgumentException("Notification is null");
        var registrations = (await this.repository.Get(filter, cancellationToken)
            .ConfigureAwait(false))
            .ToArray();

        await this
            .Send(notification, registrations, cancellationToken)
            .ConfigureAwait(false);
    }


    public async Task Send(INotification notification, PushRegistration[] registrations, CancellationToken cancellationToken = default)
    {
        notification = notification ?? throw new ArgumentException("Notification is null");

        var context = new NotificationBatchContext(this.logger, this.reporters, notification, cancellationToken);
        await context.OnBatchStart(registrations).ConfigureAwait(false);

        await Parallel
            .ForEachAsync(
                registrations,
                new ParallelOptions
                {
                    CancellationToken = cancellationToken,
                    MaxDegreeOfParallelism = 3 //TODO: this.config.MaxParallelization
                },
                async (reg, ct) =>
                {
                    try
                    {
                        var provider = this.providers.FirstOrDefault(x => x.CanPushTo(reg));
                        if (provider == null)
                            throw new InvalidOperationException("No provider found for platform: " + reg.Platform);

                        // TODO: retry options
                        await provider.Send(notification, reg, ct).ConfigureAwait(false);
                        await context.OnNotificationSuccess(reg).ConfigureAwait(false);

                        // TODO: this requires bool flag from provider
                        //await context.OnNotificationError(reg, NoSendException.Instance);
                    }
                    catch (Exception ex)
                    {
                        await context
                            .OnNotificationError(reg, ex)
                            .ConfigureAwait(false);
                    }
                }
            )
            .ConfigureAwait(false);

        await context
            .OnBatchCompleted()
            .ConfigureAwait(false);
    }
}