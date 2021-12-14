using System;
using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Ef;
using Shiny.Api.Push.Ef.Infrastructure;


namespace SampleWeb
{
    public class SampleDbContext : DbContext, IPushDbContext
    {
        public DbSet<DbPushRegistration> Registrations => this.Set<DbPushRegistration>();
        public DbSet<DbPushTag> Tags => this.Set<DbPushTag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddDefaultPushModels();
        }
    }
}

