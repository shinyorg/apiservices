using Microsoft.EntityFrameworkCore;


namespace Shiny.Api.Push.Ef
{
    public static class Extension
    {
        public static void AddDbContext<TDbContext>(this PushConfigurator config) where TDbContext : DbContext, IPushDbContext
        {

        }
    }
}
