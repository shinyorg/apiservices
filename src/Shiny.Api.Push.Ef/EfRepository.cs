using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Infrastructure;


namespace Shiny.Api.Push.Ef
{
    public class EfRepository : IRepository
    {
        readonly DbContext data;


        public EfRepository(DbContext data) 
        {
            this.data = data;
        }


        public async Task Save(NotificationRegistration reg)
        {
            var result = await this.data
                .Set<DbNotificationRegistration>()
                .FirstOrDefaultAsync(x => 
                    x.DeviceToken == reg.DeviceToken && 
                    x.Platform == reg.Platform
                )
                .ConfigureAwait(false);

            if (result == null)
            {
                result = new DbNotificationRegistration
                {
                    Tags = new List<DbNotificationRegistrationTag>(),
                    DateCreated = DateTimeOffset.Now
                };
                this.data.Add(result);
            }
            result.DeviceToken = reg.DeviceToken;
            result.Platform = reg.Platform;
            result.UserId = reg.UserId;
            result.DateExpiry = reg.DateExpiry;

            result.Tags.Clear();
            foreach (var tag in reg.Tags)
            {
                result.Tags.Add(new DbNotificationRegistrationTag
                {
                    Value = tag
                });
            }

            await this.data
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }


        public async Task<IEnumerable<NotificationRegistration>> Get(PushFilter? filter)
        {
            var regs = await this
                .FindRegistrations(filter, true)
                .ConfigureAwait(false);

            return regs.Select(x => new NotificationRegistration
            {
                DeviceToken = x.DeviceToken,
                Platform = x.Platform,
                UserId = x.UserId,
                DateExpiry = x.DateExpiry,
                DateCreated = x.DateCreated,
                Tags = x.Tags.Select(x => x.Value).ToArray()
            });
        }


        public async Task Remove(PushFilter filter)
        {
            var tokens = await this
                .FindRegistrations(filter, false)
                .ConfigureAwait(false);

            if (tokens.Count > 0)
            { 
                this.data.RemoveRange(tokens);
                await this.data.SaveChangesAsync().ConfigureAwait(false);
            }
        }


        Task<List<DbNotificationRegistration>> FindRegistrations(PushFilter? filter, bool includeTags)
        {
            var query = this.data.Set<DbNotificationRegistration>().AsQueryable();
            if (!String.IsNullOrWhiteSpace(filter?.UserId))
                query = query.Where(x => x.UserId == filter.UserId);

            if (filter?.Platform != null)
                query = query.Where(x => x.Platform == filter.Platform);

            if (filter?.DeviceToken != null)
                query = query.Where(x => x.DeviceToken == filter.DeviceToken);

            if ((filter?.Tags?.Length ?? 0) > 0)
                query = query.Where(x => x.Tags.Any(y => filter!.Tags!.Any(tag => y.Value == tag)));

            if (includeTags)
                query = query.Include(x => x.Tags);

            return query.ToListAsync();
        }
    }
}