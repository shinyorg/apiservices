using Microsoft.EntityFrameworkCore;


namespace Shiny.Api.Push.Ef
{
    public interface IPushDbContext
    {
        DbSet<DbNotificationRegistration> Registrations { get; }
        DbSet<DbNotificationRegistrationTag> Tags { get; }
    }
}
