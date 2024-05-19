using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Shiny.Extensions.EntityFramework.Tests;


public partial class EfTests : IDisposable
{
    readonly TestContextInfoProvider contextProvider;
    readonly IServiceProvider serviceProvider;

    public EfTests()
    {
        this.contextProvider = new();
        
        var services = new ServiceCollection();
        services.AddDbContextAuditing(this.contextProvider);
        // services.AddDbContextQueryLogging(this.contextProvider, TimeSpan.Zero);
        
        services.AddDbContext<TestDbContext>((sp, opts) => opts
            .UseNpgsql(new NpgsqlDataSourceBuilder("User ID=sa;Password=Blargh911!;Host=localhost;Port=5432;Database=AuditUnitTests;Pooling=true;Connection Lifetime=30;").Build())
            .UseAuditing(sp)
            // .UseQueryLogging(sp)
            // .UseSqlite("Data Source=test.db")
        );
        this.serviceProvider = services.BuildServiceProvider();

        using var scope = this.serviceProvider.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        data.Database.EnsureDeleted();
        data.Database.EnsureCreated();
        
        // TODO: seed
    }

    async Task DoDb(Func<TestDbContext, Task> task)
    {
        using var scope = this.serviceProvider.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        await task(data);
    }
    
    
    public void Dispose() => (this.serviceProvider as IDisposable)?.Dispose();
}

public class TestContextInfoProvider : IContextInfoProvider
{
    public string? AppLocation { get; set; } = "UNIT TESTS";
    public string? Tenant { get; set; } = "Test Tenant";
    public string? UserIdentifier { get; set; } = "Test User";
    public string? UserIpAddress { get; set; } = "0.0.0.0";
}