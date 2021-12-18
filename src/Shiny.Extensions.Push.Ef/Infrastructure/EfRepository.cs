using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.Push.Infrastructure;


namespace Shiny.Extensions.Push.Ef.Infrastructure
{
    public class EfRepository<TDbContext> : IRepository where TDbContext : DbContext, IPushDbContext
    {
        readonly IDbContextFactory<TDbContext> contextFactory;

        public EfRepository(IDbContextFactory<TDbContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }


        public async Task Save(PushRegistration reg, CancellationToken cancelToken = default)
        {
            using (var data = await this.contextFactory.CreateDbContextAsync(cancelToken).ConfigureAwait(false))
            {
                var result = await data
                    .Set<DbPushRegistration>()
                    .Include(x => x.Tags)
                    .FirstOrDefaultAsync(
                        x => x.DeviceToken == reg.DeviceToken && x.Platform == reg.Platform,
                        cancelToken
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
                result.DateUpdated = DateTimeOffset.UtcNow;

                result.Tags.Clear();
                if (reg.Tags != null)
                {
                    foreach (var tag in reg.Tags)
                    {
                        result.Tags.Add(new DbPushTag
                        {
                            Value = tag
                        });
                    }
                }

                await data
                    .SaveChangesAsync(cancelToken)
                    .ConfigureAwait(false);
            }
        }


        public async Task<IEnumerable<PushRegistration>> Get(PushFilter? filter, CancellationToken cancelToken = default)
        {
            using (var data = await this.contextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var regs = await this
                    .FindRegistrations(data, filter, true, cancelToken)
                    .ConfigureAwait(false);

                return regs.Select(x => new PushRegistration
                {
                    DeviceToken = x.DeviceToken,
                    Platform = x.Platform,
                    UserId = x.UserId,
                    DateUpdated = x.DateUpdated,
                    DateCreated = x.DateCreated,
                    Tags = x.Tags.Select(x => x.Value).ToArray()
                });
            }
        }


        public async Task Remove(PushFilter filter, CancellationToken cancelToken = default)
        {
            using (var data = await this.contextFactory.CreateDbContextAsync(cancelToken).ConfigureAwait(false))
            {
                var tokens = await this
                    .FindRegistrations(data, filter, false, cancelToken)
                    .ConfigureAwait(false);

                if (tokens.Count > 0)
                {
                    data.RemoveRange(tokens);
                    await data
                        .SaveChangesAsync(cancelToken)
                        .ConfigureAwait(false);
                }
            }
        }


        public async Task RemoveBatch(PushRegistration[] pushRegistrations, CancellationToken cancelToken = default)
        {
            using (var data = await this.contextFactory.CreateDbContextAsync(cancelToken).ConfigureAwait(false))
            {
                foreach (var reg in pushRegistrations)
                {
                    data.Attach(reg);
                    data.Remove(reg);
                }
                await data
                    .SaveChangesAsync(cancelToken)
                    .ConfigureAwait(false);
            }
        }


        Task<List<DbPushRegistration>> FindRegistrations(TDbContext data, PushFilter? filter, bool includeTags, CancellationToken cancelToken)
        {
            var query = data.Set<DbPushRegistration>().AsQueryable();
            if (!String.IsNullOrWhiteSpace(filter?.UserId))
                query = query.Where(x => x.UserId == filter.UserId);

            if ((filter?.Platform ?? PushPlatforms.All) != PushPlatforms.All)
                query = query.Where(x => x.Platform == filter!.Platform);

            if (filter?.DeviceToken != null)
                query = query.Where(x => x.DeviceToken == filter.DeviceToken);

            if ((filter?.Tags?.Length ?? 0) > 0)
                query = query.Where(x => x.Tags.Any(y => filter!.Tags!.Any(tag => y.Value == tag)));

            if (includeTags)
                query = query.Include(x => x.Tags);

            return query.ToListAsync(cancelToken);
        }
    }
}