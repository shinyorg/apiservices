namespace Shiny.Api.Push;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface INotificationReporter
{
    Task OnBatchStart(
        Guid batchId,
        IReadOnlyCollection<PushRegistration> registrations,
        Notification notification
    );


    Task OnNotificationSuccess(
        Guid batchId,
        PushRegistration registration,
        Notification notification
    );


    Task OnNotificationError(
        Guid batchId,
        PushRegistration registration,
        Notification notification,
        Exception exception
    );


    Task OnBatchCompleted(
        Guid batchId,
        IReadOnlyCollection<PushRegistration> success,
        IReadOnlyCollection<(PushRegistration, Exception)> failures,
        Notification notification
    );
}
