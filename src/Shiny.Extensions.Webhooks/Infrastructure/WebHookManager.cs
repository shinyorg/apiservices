using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Extensions.Webhooks.Infrastructure;


// TODO: hash verification, auditing or at least see batch
public class WebHookManager : IWebHookManager
{
    readonly IRepository repository;
    readonly IRunner runner;
    readonly WebHookRunnerConfig configuration;


    public WebHookManager(
        IRepository repository,
        IRunner runner,
        WebHookRunnerConfig configuration
    )
    {
        this.repository = repository;
        this.runner = runner;
        this.configuration = configuration;
    }


    public Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName)
        => this.repository.GetRegistrations(eventName);


    public async Task<T?> Request<T>(WebHookRegistration registration, object? args, CancellationToken cancelToken = default)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(registration.ExecutionTimeoutSeconds ?? this.configuration.DefaultWaitTime));
        using var reg = cancelToken.Register(() => timeout.Cancel());

        var result = await this.runner
            .Request<T>(
                registration,
                args,
                timeout.Token
            )
            .ConfigureAwait(false);

        return result;
    }


    public async Task<SendBatchResult> Send(string eventName, object? args, CancellationToken cancelToken = default)
    {
        var success = new List<WebHookRegistration>();
        var errors = new List<(WebHookRegistration, Exception)>();

        var registrations = await this.repository
            .GetRegistrations(eventName)
            .ConfigureAwait(false);

        await Parallel
            .ForEachAsync(
                registrations,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = this.configuration.MaxDegreeOfParallelism
                },
                async (reg, token) =>
                {
                    try
                    {
                        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(reg.ExecutionTimeoutSeconds ?? this.configuration.DefaultWaitTime));
                        using var cancelSub = cancelToken.Register(() => timeout.Cancel());

                        await this.runner
                            .Send(reg, args, token)
                            .ConfigureAwait(false);

                        lock (success)
                            success.Add(reg);
                    }
                    catch (Exception ex)
                    {
                        lock (errors)
                            errors.Add((reg, ex));
                    }
                }
            )
            .ConfigureAwait(false);

        return new SendBatchResult(success, errors);
    }


    public Task Subscribe(WebHookRegistration registration)
        => this.repository.Subscribe(registration);


    public Task UnSubscribe(Guid registrationId)
        => this.repository.UnSubscribe(registrationId);
}