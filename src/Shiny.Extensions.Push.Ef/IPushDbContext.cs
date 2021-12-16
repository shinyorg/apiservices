using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.Push.Ef.Infrastructure;


namespace Shiny.Extensions.Push.Ef
{
    public interface IPushDbContext
    {
        DbSet<DbPushRegistration> Registrations { get; }
        DbSet<DbPushTag> Tags { get; }
    }
}
