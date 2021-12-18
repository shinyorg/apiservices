using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push
{ 
    public interface INotificationReporter
    {
        Task OnBatchStart(
            Guid batchId,
            IReadOnlyCollection<PushRegistration> registrations,
            Notification notification,
            CancellationToken cancelToken
        );


        Task OnNotificationSuccess(
            Guid batchId,
            PushRegistration registration,
            Notification notification,
            CancellationToken cancelToken
        );


        Task OnNotificationError(
            Guid batchId,
            PushRegistration registration,
            Notification notification,
            Exception exception,
            CancellationToken cancelToken
        );


        Task OnBatchCompleted(
            Guid batchId,
            IReadOnlyCollection<PushRegistration> success,
            IReadOnlyCollection<(PushRegistration Registration, Exception Exception)> failures,
            Notification notification,
            CancellationToken cancelToken
        );
    }
}
