using System;
using Microsoft.EntityFrameworkCore;
using Shiny.Api.Push.Ef;
using Shiny.Api.Push.Ef.Infrastructure;


namespace SampleWeb
{
    public class SampleDbContext : DbContext, IPushDbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }


        public DbSet<DbPushRegistration> Registrations => this.Set<DbPushRegistration>();
        public DbSet<DbPushTag> Tags => this.Set<DbPushTag>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var reg = modelBuilder.Entity<DbPushRegistration>();
            reg.ToTable("PushRegistrations");
            reg.HasKey(x => x.Id);
            reg
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("PushRegistrationId");

            reg.Property(x => x.Platform);
            reg.Property(x => x.DeviceToken).HasMaxLength(512).IsRequired();
            reg.Property(x => x.UserId).HasMaxLength(50).IsRequired(false);
            reg
                .HasMany(x => x.Tags)
                .WithOne(x => x.PushRegistration)
                .HasForeignKey(x => x.PushRegistrationId);

            var tag = modelBuilder.Entity<DbPushTag>();
            tag.ToTable("PushTags");
            tag.HasKey(x => x.Id);
            tag
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("PushTagId");

            tag.Property(x => x.Value).HasMaxLength(50).IsRequired();
            tag.Property(x => x.PushRegistrationId).IsRequired();
            tag
                .HasOne(x => x.PushRegistration)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.PushRegistrationId);
        }
    }
}

