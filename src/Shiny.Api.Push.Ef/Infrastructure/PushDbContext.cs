using Microsoft.EntityFrameworkCore;


namespace Shiny.Api.Push.Ef.Infrastructure
{
    public class PushDbContext : DbContext
    {
        public DbSet<DbNotificationRegistration> Registrations { get; set; }
        public DbSet<DbNotificationRegistrationTag> Tags { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var notif = modelBuilder.Entity<DbNotificationRegistration>();
            var tag = modelBuilder.Entity<DbNotificationRegistrationTag>();

            base.OnModelCreating(modelBuilder);
        }
    }
}