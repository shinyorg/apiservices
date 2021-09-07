using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Api.Push.Management
{
    public interface IPushManager
    {
        // TODO: what about formatting each message per user
        // TODO: store messages, attempts, etc
        Task Send(Notification notification, string? userId, PushType? type, params string[] tags);
        Task<IEnumerable<NotificationRegistration>> GetRegistrations(string? userId, PushType? type, params string[] tags);

        Task Register(NotificationRegistration registration);
        Task UnRegister(PushType pushType, string registrationToken);
        Task UnRegister(string? userId, params string[] tags);
    }
}
