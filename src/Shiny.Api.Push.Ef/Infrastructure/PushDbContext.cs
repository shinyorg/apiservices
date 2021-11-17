using Microsoft.EntityFrameworkCore;


namespace Shiny.Api.Push.Ef.Infrastructure
{
    public class PushDbContext : DbContext
    {
        public DbSet<DbPushRegistration> Registrations { get; set; }
        public DbSet<DbPushTag> Tags { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var notif = modelBuilder.Entity<DbPushRegistration>();
            var tag = modelBuilder.Entity<DbPushTag>();

            base.OnModelCreating(modelBuilder);
        }
    }
}