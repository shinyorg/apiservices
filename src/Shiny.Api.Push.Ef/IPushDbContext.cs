using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Ef.Infrastructure;


namespace Shiny.Api.Push.Ef
{
    public interface IPushDbContext
    {
        DbSet<DbPushRegistration> Registrations { get; }
        DbSet<DbPushTag> Tags { get; }
    }
}
