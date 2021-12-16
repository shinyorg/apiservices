using Microsoft.Extensions.Logging;
using Shiny.Extensions.Push.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Extensions
{
    public class AutoCleanupNotificationReporter : NotificationReporter
    {
        readonly IRepository repository;
        readonly ILogger logger;


        public AutoCleanupNotificationReporter(IRepository repository, ILogger<AutoCleanupNotificationReporter> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }


        public override async Task OnBatchCompleted(Guid batchId, IReadOnlyCollection<PushRegistration> success, IReadOnlyCollection<(PushRegistration Registration, Exception Exception)> failures, Notification notification)
        {
            var noSends = failures
                .Where(x => x.Exception is NoSendException)
                .Select(x => x.Registration)
                .ToArray();

            if (noSends.Length > 0)
            {
                try
                {
                    this.logger.LogInformation("Removing {0} failed senders", noSends.Length);
                    await this.repository.RemoveBatch(noSends).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    this.logger.LogWarning(ex, "Failed to cleanup failed senders");
                }
            }
        }
    }
}
