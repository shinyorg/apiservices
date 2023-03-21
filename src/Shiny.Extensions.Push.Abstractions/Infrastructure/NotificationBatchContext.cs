using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Shiny.Extensions.Push.Infrastructure;


public class NotificationBatchContext
{
    readonly Guid batchId;
    readonly ILogger logger;
    readonly INotification notification;
    readonly List<PushRegistration> success = new();
    readonly List<(PushRegistration, Exception)> failures = new();
    readonly IEnumerable<INotificationReporter> reporters;
    readonly CancellationToken cancellationToken;


    public NotificationBatchContext(
        ILogger logger,
        IEnumerable<INotificationReporter> reporters,
        INotification notification,
        CancellationToken cancelToken
    )
    {
        this.batchId = Guid.NewGuid();
        this.logger = logger;
        this.reporters = reporters;
        this.notification = notification;
        this.cancellationToken = cancelToken;
    }


    public async Task OnBatchStart(IEnumerable<PushRegistration> registrations)
    {
        var list = registrations.ToList();
        this.logger.LogInformation("Starting Batch {0} - Sending to {1} registrations", this.batchId, list.Count);
        await this.Wrap(x => x.OnBatchStart(this.batchId, list, this.notification, this.cancellationToken)).ConfigureAwait(false);
    }


    public async Task OnBatchCompleted()
    {
        this.logger.LogInformation("Batch {0} completed - Success: {1} - Failures: {2}", this.batchId, this.success.Count, this.failures.Count);
        await this.Wrap(x => x.OnBatchCompleted(this.batchId, this.success, this.failures, this.notification, this.cancellationToken)).ConfigureAwait(false);
    }


    public async Task OnNotificationError(PushRegistration registration, Exception exception)
    {
        lock (this.failures)
            this.failures.Add((registration, exception));

        this.logger.LogWarning(exception, $"Batch {this.batchId} Notification Error - Device Token: {registration.DeviceToken}");
        await this.Wrap(x => x.OnNotificationError(this.batchId, registration, this.notification, exception, this.cancellationToken)).ConfigureAwait(false);
    }


    public async Task OnNotificationSuccess(PushRegistration registration)
    {
        lock (this.success)
            this.success.Add(registration);

        this.logger.LogInformation("Batch {0} - Success Device Token: {1}", this.batchId, registration.DeviceToken);
        var s = success.ToArray();
        var f = failures.ToArray();

        await this.Wrap(x => x.OnNotificationSuccess(this.batchId, registration, this.notification, this.cancellationToken)).ConfigureAwait(false);
    }


    async Task Wrap(Func<INotificationReporter, Task> taskFunc, [CallerMemberName] string? caller = null)
    {
        if (!this.reporters.Any())
            return;

        try
        {
            var tasks = this.reporters
                .Select(x => taskFunc(x))
                .ToArray();

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError("Error with INotificationReporter." + caller, ex);
        }
    }
}