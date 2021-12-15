using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Infrastructure;


namespace Shiny.Api.Push.Ef.Infrastructure
{
    public class EfRepository<TDbContext> : IRepository where TDbContext : DbContext, IPushDbContext
    {
        readonly IDbContextFactory<TDbContext> contextFactory;

        public EfRepository(IDbContextFactory<TDbContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }


        public async Task Save(PushRegistration reg)
        {
            using (var data = await this.contextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var result = await data
                    .Set<DbPushRegistration>()
                    .FirstOrDefaultAsync(x =>
                        x.DeviceToken == reg.DeviceToken &&
                        x.Platform == reg.Platform
                    )
                    .ConfigureAwait(false);

                if (result == null)
                {
                    result = new DbPushRegistration
                    {
                        Tags = new List<DbPushTag>(),
                        DateCreated = DateTimeOffset.Now
                    };
                    data.Add(result);
                }
                result.DeviceToken = reg.DeviceToken;
                result.Platform = reg.Platform;
                result.UserId = reg.UserId;
                result.DateExpiry = reg.DateExpiry;

                result.Tags.Clear();
                foreach (var tag in reg.Tags)
                {
                    result.Tags.Add(new DbPushTag
                    {
                        Value = tag
                    });
                }

                await data
                    .SaveChangesAsync()
                    .ConfigureAwait(false);
            }
        }


        public async Task<IEnumerable<PushRegistration>> Get(PushFilter? filter)
        {
            using (var data = await this.contextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var regs = await this
                    .FindRegistrations(data, filter, true)
                    .ConfigureAwait(false);

                return regs.Select(x => new PushRegistration
                {
                    DeviceToken = x.DeviceToken,
                    Platform = x.Platform,
                    UserId = x.UserId,
                    DateExpiry = x.DateExpiry,
                    DateCreated = x.DateCreated,
                    Tags = x.Tags.Select(x => x.Value).ToArray()
                });
            }
        }


        public async Task Remove(PushFilter filter)
        {
            using (var data = await this.contextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var tokens = await this
                    .FindRegistrations(data, filter, false)
                    .ConfigureAwait(false);

                if (tokens.Count > 0)
                {
                    data.RemoveRange(tokens);
                    await data.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }


        Task<List<DbPushRegistration>> FindRegistrations(TDbContext data, PushFilter? filter, bool includeTags)
        {
            var query = data.Set<DbPushRegistration>().AsQueryable();
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