using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Api.Push.Management
{
    public interface IPushManager
    {
        // TODO: what about formatting each message per user
        // TODO: store messages, attempts, etc
        Task Send(Notification notification, PushFilter? filter);
        Task<IEnumerable<NotificationRegistration>> GetRegistrations(PushFilter? filter);

        Task Register(NotificationRegistration registration);
        Task UnRegister(PushFilter filter);
    }
}
