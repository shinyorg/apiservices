using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push
{
    public interface IPushManager
    {
        // TODO: update a registration tags
        // TODO: what about formatting each message per user
        // TODO: store messages, attempts, etc
        Task Send(Notification notification, PushFilter? filter);
        Task<IEnumerable<PushRegistration>> GetRegistrations(PushFilter? filter);

        Task Register(PushRegistration registration);
        Task UnRegister(PushFilter filter);
    }
}
