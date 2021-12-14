namespace Shiny.Api.Push.Ef.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public static class EfExtensions
{
    public static void AddDefaultPushModels(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DbPushRegistrationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DbPushTagEntityTypeConfiguration());
    }
}

public class DbPushRegistrationEntityTypeConfiguration : IEntityTypeConfiguration<DbPushRegistration>
{
    public void Configure(EntityTypeBuilder<DbPushRegistration> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.Tags)
            .WithOne(x => x.Registration)
            .HasForeignKey(x => x.Id);
    }
}


public class DbPushTagEntityTypeConfiguration : IEntityTypeConfiguration<DbPushTag>
{
    public void Configure(EntityTypeBuilder<DbPushTag> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Registration)
            .WithMany(x => x.Tags)
            .HasForeignKey(x => x.NotificationRegistrationId);
    }
}