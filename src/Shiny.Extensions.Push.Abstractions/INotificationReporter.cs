using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Extensions.Push;


public interface INotificationReporter
{
    Task OnBatchStart(
        Guid batchId,
        IReadOnlyCollection<PushRegistration> registrations,
        INotification notification,
        CancellationToken cancelToken
    );


    Task OnNotificationSuccess(
        Guid batchId,
        PushRegistration registration,
        INotification notification,
        CancellationToken cancelToken
    );


    Task OnNotificationError(
        Guid batchId,
        PushRegistration registration,
        INotification notification,
        Exception exception,
        CancellationToken cancelToken
    );


    Task OnBatchCompleted(
        Guid batchId,
        IReadOnlyCollection<PushRegistration> success,
        IReadOnlyCollection<(PushRegistration Registration, Exception Exception)> failures,
        INotification notification,
        CancellationToken cancelToken
    );
}
