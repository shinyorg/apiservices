using Shiny.Api.Push.Management.Infrastructure;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Api.Push.Management
{
    public class DbPushManager : IPushManager
    {
        readonly PushDbContext data;
        readonly IEnumerable<IPushProvider> providers;


        public DbPushManager(PushDbContext data, IEnumerable<IPushProvider> providers)
        {
            this.data = data;
            this.providers = providers;
        }


        public Task<IEnumerable<NotificationRegistration>> GetRegistrations(string? userId, PushType? type, params string[] tags)
        {
            throw new NotImplementedException();
        }

        public Task Register(NotificationRegistration registration)
        {
            throw new NotImplementedException();
        }

        public Task Send(Notification notification, string? userId, PushType? type, params string[] tags)
        {
            throw new NotImplementedException();
        }

        public Task UnRegister(PushType pushType, string registrationToken)
        {
            throw new NotImplementedException();
        }

        public Task UnRegister(string? userId, params string[] tags)
        {
            throw new NotImplementedException();
        }
    }
}
