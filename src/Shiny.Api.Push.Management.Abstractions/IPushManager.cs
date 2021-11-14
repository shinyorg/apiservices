using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Api.Push.Management
{
    public interface IPushManager
    {
        // TODO: string for what OS it manages
        // TODO: what about formatting each message per user
        // TODO: store messages, attempts, etc
        Task Send(Notification notification, string? userId, PushPlatform? platform, params string[] tags);
        Task<IEnumerable<NotificationRegistration>> GetRegistrations(string? userId, PushPlatform? platform, params string[] tags);

        Task Register(NotificationRegistration registration);
        Task UnRegister(PushPlatform? platform, string registrationToken);
        Task UnRegister(string? userId, params string[] tags);
    }
}
