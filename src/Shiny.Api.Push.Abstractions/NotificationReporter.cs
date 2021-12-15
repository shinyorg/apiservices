using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Api.Push
{
    public abstract class NotificationReporter : INotificationReporter
    {
        public virtual Task OnBatchStart(
            Guid batchId,
            IReadOnlyCollection<PushRegistration> registrations,
            Notification notification
        ) => Task.CompletedTask;


        public virtual Task OnNotificationSuccess(
            Guid batchId,
            PushRegistration registration,
            Notification notification
        ) => Task.CompletedTask;


        public virtual Task OnNotificationError(
            Guid batchId,
            PushRegistration registration,
            Notification notification,
            Exception exception
        ) => Task.CompletedTask;


        public virtual Task OnBatchCompleted(
            Guid batchId,
            IReadOnlyCollection<PushRegistration> success,
            IReadOnlyCollection<(PushRegistration, Exception)> failures,
            Notification notification
        ) => Task.CompletedTask;
    }
}