using Microsoft.EntityFrameworkCore;


namespace Shiny.Api.Push.Ef
{
    public interface IPushDbContext
    {
        DbSet<DbPushRegistration> Registrations { get; }
        DbSet<DbPushTag> Tags { get; }
    }
}
