using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Ef.Infrastructure;


namespace Shiny.Api.Push.Ef
{
    public static class Extension
    {
        public static PushConfigurator UseEfRepository<TDbContext>(this PushConfigurator config) where TDbContext : DbContext, IPushDbContext
            => config.UseRepository<EfRepository<TDbContext>>();
    }
}
