using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Management.Models;


namespace Shiny.Api.Push.Management.Infrastructure
{
    public class PushDbContext : DbContext
    {
        public DbSet<NotificationRegistrationModel> Registrations { get; set; }
        public DbSet<NotificationRegistrationTag> Tags { get; set; }


        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var notif = modelBuilder.Entity<NotificationRegistrationModel>();
            var tag = modelBuilder.Entity<NotificationRegistrationTag>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
