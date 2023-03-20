using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Extensions.Push.Extensions;


public class BatchTimeNotificationReporter : NotificationReporter
{
    readonly Dictionary<Guid, Stopwatch> timers = new();
    readonly ILogger logger;

    public BatchTimeNotificationReporter(ILogger<BatchTimeNotificationReporter> logger) => this.logger = logger;


    public override Task OnBatchStart(Guid batchId, IReadOnlyCollection<PushRegistration> registrations, INotification notification, CancellationToken cancelToken = default)
    {
        lock (this.timers)
        {
            var sw = new Stopwatch();
            sw.Start();
            this.timers.Add(batchId, sw);
        }
        return Task.CompletedTask;
    }


    public override Task OnBatchCompleted(
        Guid batchId, 
        IReadOnlyCollection<PushRegistration> success, 
        IReadOnlyCollection<(PushRegistration Registration, Exception Exception)> failures, 
        INotification notification,
        CancellationToken cancelToken = default
    )
    {
        if (this.timers.ContainsKey(batchId))
        {
            Stopwatch sw;
            lock (this.timers)
            {
                sw = this.timers[batchId];
                sw.Stop();
                this.timers.Remove(batchId);
            }

            this.logger.LogInformation(
                "BatchID {0} sent to {1} total receipient(s) and took {2}",
                batchId,
                success.Count + failures.Count,
                sw.Elapsed.ToString()
            );
        }
        return Task.CompletedTask;
    }
}
