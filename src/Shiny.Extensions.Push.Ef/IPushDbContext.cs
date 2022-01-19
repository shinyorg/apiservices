using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.Push.Ef;


namespace Shiny.Extensions.Push
{
    public interface IPushDbContext
    {
        DbSet<DbPushRegistration> Registrations { get; }
        DbSet<DbPushTag> Tags { get; }
    }
}
