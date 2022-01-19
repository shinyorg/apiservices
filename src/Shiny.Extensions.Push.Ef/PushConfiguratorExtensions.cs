using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.Push.Ef;


namespace Shiny.Extensions.Push
{
    public static class PushConfiguratorExtensions
    {
        public static PushConfigurator UseEfRepository<TDbContext>(this PushConfigurator config) where TDbContext : DbContext, IPushDbContext
            => config.UseRepository<EfRepository<TDbContext>>();
    }
}
