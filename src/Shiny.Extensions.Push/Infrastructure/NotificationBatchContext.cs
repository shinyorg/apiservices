using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{ 
    public class NotificationBatchContext
    {
        readonly Guid batchId;
        readonly ILogger logger;
        readonly Notification notification;
        readonly List<PushRegistration> success = new();
        readonly List<(PushRegistration, Exception)> failures = new();
        readonly List<INotificationReporter> reporters;
        readonly CancellationToken cancellationToken;


        public NotificationBatchContext(ILogger logger,
                                        List<INotificationReporter> reporters,
                                        Notification notification,
                                        CancellationToken cancelToken)
        {
            this.batchId = Guid.NewGuid();
            this.logger = logger;
            this.reporters = reporters;
            this.notification = notification;
            this.cancellationToken = cancelToken;
        }


        public GoogleConfiguration? GoogleConfiguration { get; internal set; }
        public AppleConfiguration? AppleConfiguration { get; internal set; }


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
            this.failures.Add((registration, exception));

            this.logger.LogWarning(exception, $"Batch {this.batchId} Notification Error - Device Token: {registration.DeviceToken}");
            await this.Wrap(x => x.OnNotificationError(this.batchId, registration, this.notification, exception, this.cancellationToken)).ConfigureAwait(false);
        }


        public async Task OnNotificationSuccess(PushRegistration registration)
        {
            this.success.Add(registration);

            this.logger.LogInformation("Batch {0} - Success Device Token: {1}", this.batchId, registration.DeviceToken);
            var s = success.ToArray();
            var f = failures.ToArray();

            await this.Wrap(x => x.OnNotificationSuccess(this.batchId, registration, this.notification, this.cancellationToken)).ConfigureAwait(false);
        }


        async Task Wrap(Func<INotificationReporter, Task> taskFunc, [CallerMemberName] string? caller = null)
        {
            if (this.reporters.Count == 0)
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
}