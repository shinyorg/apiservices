using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<IEnumerable<NotificationRegistration>> GetRegistrations(PushFilter filter)
        { 
            var regs = await this.FindRegistrations(filter).ConfigureAwait(false);

            return regs.Select(x => new NotificationRegistration
            {

            });
        }


        public Task Register(NotificationRegistration registration) 
        {
            this.data.Registrations.Add(new Models.NotificationRegistrationModel
            {

            });

            return this.data.SaveChangesAsync();
        }


        public async Task Send(Notification notification, PushFilter filter)
        {
            var tokens = await this.FindRegistrations(filter).ConfigureAwait(false);
            if (tokens.Count > 0)
            {
                // TODO: get provider per 
                // TODO: trigger send
            }
        }


        public async Task UnRegister(PushFilter filter)
        {
            var tokens = await this.FindRegistrations(filter).ConfigureAwait(false);
            if (tokens.Count > 0)
            { 
                this.data.RemoveRange(tokens);
                await this.data.SaveChangesAsync().ConfigureAwait(false);
            }
        }


        Task<List<Models.NotificationRegistrationModel>> FindRegistrations(PushFilter filter)
        {
            var query = this.data.Registrations.AsQueryable();
            if (!String.IsNullOrWhiteSpace(filter.UserId))
                query = query.Where(x => x.UserId == filter.UserId);

            if (filter.Platform != null)
                query = query.Where(x => x.Platform == filter.Platform);

            if (filter.DeviceToken != null)
                query = query.Where(x => x.DeviceToken == filter.DeviceToken);

            if ((filter.Tags?.Length ?? 0) > 0)
                query = query.Where(x => x.Tags.Any(y => filter.Tags!.Any(tag => y.Value == tag)));

            return query.ToListAsync();
        }
    }
}
