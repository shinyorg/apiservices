using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shiny.Auditing;

namespace Shiny;


public static class EntityFrameworkExtensions
{
    public static ModelConfigurationBuilder SetDefaultStringLength(this ModelConfigurationBuilder configurationBuilder, int length = 50, bool unicode = true)
    {
        configurationBuilder
            .Properties<string>()
            .AreUnicode(unicode)
            .HaveMaxLength(length);

        return configurationBuilder;
    }
    
    
    // modelBuilder
    //     .Entity<Person>(
    // eb =>
    // {
    //     eb.Property(p => p.Addresses).HasConversion(
    //
    //         v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
    //         v => JsonConvert.DeserializeObject<IList<Address>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
    //     );
    // });
    
    public static ModelBuilder MapAuditing(this ModelBuilder modelBuilder)
    {
        var map = modelBuilder.Entity<AuditEntry>();

        map.HasKey(x => x.Id);
        map.Property(x => x.Id).ValueGeneratedOnAdd();
        map.Property(x => x.EntityId).HasMaxLength(100);
        map.Property(x => x.EntityType).HasMaxLength(255);
        map.Property(x => x.Operation);
        map.Property(x => x.Timestamp);
        map.Property(x => x.ChangeSet);
        
        map.Property(x => x.UserIdentifier).HasMaxLength(50);
        map.Property(x => x.UserIpAddress).HasMaxLength(39);
        map.Property(x => x.Tenant).HasMaxLength(50);
        map.Property(x => x.AppLocation).HasMaxLength(1024);
        
        return modelBuilder;
    }    
 

    public static ModelBuilder MapEasyPropertyIds(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entityType.GetProperties().FirstOrDefault(x => x.Name.Equals("Id"));
            if (idProperty != null && idProperty.GetColumnName().Equals("Id"))
                idProperty.SetColumnName(entityType.ClrType.Name + "Id");
        }
        return modelBuilder;
    }
}