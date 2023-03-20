namespace Shiny.Extensions.Push;


public abstract class NotificationReporter : INotificationReporter
{
    public virtual Task OnBatchStart(
        Guid batchId,
        IReadOnlyCollection<PushRegistration> registrations,
        INotification notification,
        CancellationToken cancelToken
    ) => Task.CompletedTask;


    public virtual Task OnNotificationSuccess(
        Guid batchId,
        PushRegistration registration,
        INotification notification,
        CancellationToken cancelToken
    ) => Task.CompletedTask;


    public virtual Task OnNotificationError(
        Guid batchId,
        PushRegistration registration,
        INotification notification,
        Exception exception,
        CancellationToken cancelToken
    ) => Task.CompletedTask;


    public virtual Task OnBatchCompleted(
        Guid batchId,
        IReadOnlyCollection<PushRegistration> success,
        IReadOnlyCollection<(PushRegistration Registration, Exception Exception)> failures,
        INotification notification,
        CancellationToken cancelToken
    ) => Task.CompletedTask;
}