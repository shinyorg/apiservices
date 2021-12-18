using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.Push;
using Shiny.Extensions.Push.Ef;
using Shiny.Extensions.Push.Ef.Infrastructure;


namespace Shiny
{
    public static class Extension
    {
        public static PushConfigurator UseEfRepository<TDbContext>(this PushConfigurator config) where TDbContext : DbContext, IPushDbContext
            => config.UseRepository<EfRepository<TDbContext>>();
    }
}
