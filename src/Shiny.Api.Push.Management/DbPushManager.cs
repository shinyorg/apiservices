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


        public Task<IEnumerable<NotificationRegistration>> GetRegistrations(string? userId, PushPlatform? platform, params string[] tags)
        {
            throw new NotImplementedException();
        }

        public Task Register(NotificationRegistration registration) 
        {
            this.data.Registrations.Add(new Models.NotificationRegistrationModel
            {

            });

            return this.data.SaveChangesAsync();
        }

        public Task Send(Notification notification, string? userId, PushPlatform? platform, params string[] tags)
        {
            throw new NotImplementedException();
        }

        public Task UnRegister(PushPlatform platform, string registrationToken)
        {
            throw new NotImplementedException();
        }

        public Task UnRegister(string? userId, params string[] tags)
        {
            throw new NotImplementedException();
        }
    }
}
