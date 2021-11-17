﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Api.Push
{
    public interface IPushManager
    {
        // TODO: update a registration tags
        // TODO: what about formatting each message per user
        // TODO: store messages, attempts, etc
        Task Send(Notification notification, PushFilter? filter);
        Task<IEnumerable<NotificationRegistration>> GetRegistrations(PushFilter? filter);

        Task Register(NotificationRegistration registration);
        Task UnRegister(PushFilter filter);
    }
}