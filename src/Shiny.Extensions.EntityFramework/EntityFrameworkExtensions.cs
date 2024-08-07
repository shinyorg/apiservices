using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Auditing;
using Shiny.QueryLog;

namespace Shiny;


public static class EntityFrameworkExtensions
{
    public static IServiceCollection AddDbContextAuditing<TContextProvider>(this IServiceCollection services) where TContextProvider : class, IContextInfoProvider
    {
        services.AddScoped<IContextInfoProvider, TContextProvider>();
        services.AddScoped<AuditSaveChangesInterceptor>();
        return services;
    }


    public static IServiceCollection AddDbContextAuditing(
        this IServiceCollection services,
        IContextInfoProvider contextProvider
    ) => services.AddScoped(_ => new AuditSaveChangesInterceptor(contextProvider));
    
    
    public static DbContextOptionsBuilder UseAuditing(this DbContextOptionsBuilder builder, IServiceProvider scope)
    {
        var interceptor = scope.GetRequiredService<AuditSaveChangesInterceptor>();
        return builder.AddInterceptors(interceptor);
    }


    // public static IServiceCollection AddDbContextQueryLogging<T>(this IServiceCollection services, TimeSpan minLogDuration) where T : class, IContextInfoProvider
    // {
    //     services.AddScoped<T>();
    //     services.AddScoped(sp =>
    //     {
    //         var contextInfo = sp.GetRequiredService<T>();
    //         return new QueryLogDbCommandInterceptor(contextInfo, minLogDuration);
    //     });
    //     return services;
    // }
    //
    //
    // public static IServiceCollection AddDbContextQueryLogging(
    //     this IServiceCollection services,
    //     IContextInfoProvider contextProvider,
    //     TimeSpan minLogDuration
    // ) => services.AddScoped(_ => new QueryLogDbCommandInterceptor(contextProvider, minLogDuration));
    //
    //
    // public static void UseQueryLogging(this DbContextOptionsBuilder builder, IServiceProvider scope)
    // {
    //     var interceptor = scope.GetRequiredService<QueryLogDbCommandInterceptor>();
    //     builder.AddInterceptors(interceptor);
    // } 
    
    
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
        map.Property(x => x.TableName).HasMaxLength(255);
        map.Property(x => x.Operation);
        map.Property(x => x.Timestamp);
        map.Property(x => x.ChangeSet);
        
        map.Property(x => x.UserIdentifier).HasMaxLength(50);
        map.Property(x => x.UserIpAddress).HasMaxLength(39);
        map.Property(x => x.AppLocation).HasMaxLength(1024);
        
        return modelBuilder;
    }   
    
    
    public static ModelBuilder MapSlowQueryAuditing(this ModelBuilder modelBuilder)
    {
        var map = modelBuilder.Entity<QueryLogEntry>();

        map.HasKey(x => x.Id);
        map.Property(x => x.Id).ValueGeneratedOnAdd();
        map.Property(x => x.Query).HasMaxLength(5000);
        map.Property(x => x.Duration);
        map.Property(x => x.Timestamp);
        
        map.Property(x => x.UserIdentifier).HasMaxLength(50);
        map.Property(x => x.UserIpAddress).HasMaxLength(39);
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