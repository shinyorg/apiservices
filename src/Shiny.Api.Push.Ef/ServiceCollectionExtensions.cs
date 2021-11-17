using Microsoft.EntityFrameworkCore;


namespace Shiny.Api.Push.Ef
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext<T>() where T : DbContext, IPushDbContext
        {

        }
    }
}
