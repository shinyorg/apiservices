using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shiny.Auditing;

namespace Shiny.Extensions.EntityFramework.Tests;


// TODO: test on entity updates
// TODO: test ignored properties
public class AuditTests : IDisposable
{
    readonly TestAuditInfoProvider auditProvider;
    readonly IServiceProvider serviceProvider;

    public AuditTests()
    {
        this.auditProvider = new();
        
        var services = new ServiceCollection();
        services.AddDbContext<TestDbContext>(x => x
            // .UseSqlite("Data Source=test.db")
            .UseNpgsql(new NpgsqlDataSourceBuilder("User ID=sa;Password=Blargh911!;Host=localhost;Port=5432;Database=AuditUnitTests;Pooling=true;Connection Lifetime=30;").Build())
            .AddInterceptors(new AuditSaveChangesInterceptor(this.auditProvider))
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
    
    
    [Fact]
    public async Task AddAudits()
    {
        using var scope = this.serviceProvider.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        await this.DoDb(data =>
        {
            var manu = new Manufacturer { Name = "Cadillac" };
            data.Add(manu);

            var model = new Model
            {
                Name = "X5",
                Manufacturer = manu
            };
            data.Add(model);
            return data.SaveChangesAsync();
        });

        
        await this.DoDb(async data =>
        {
            var audits = await data.AuditEntries.ToListAsync();
            audits.Count.Should().Be(2);
        
            audits.All(x => x.UserIdentifier!.Equals("Test User")).Should().BeTrue("All audits should be 'Test User'");
            audits.All(x => x.Tenant!.Equals("Test Tenant")).Should().BeTrue("All audits should be 'Test Tenant'");
            audits.All(x => x.UserIpAddress!.Equals("0.0.0.0")).Should().BeTrue("All audits should be '0.0.0.0'");
            audits.All(x => x.AppLocation!.Equals("UNIT TESTS")).Should().BeTrue("All audits should be 'UNIT TESTS'");
        
            audits.All(x => x.Operation == DbOperation.Insert).Should().BeTrue("All audits should Insert operations");

            var manu = await data.Manufacturers.FirstOrDefaultAsync();
            manu.LastEditUserIdentifier.Should().Be("Test User");
            manu.DateUpdated.Date.Should().Be(DateTimeOffset.UtcNow.Date, "DateUpdated is wrong");
            manu.DateCreated.Date.Should().Be(DateTimeOffset.UtcNow.Date, "DateCreated is wrong");
        });
    }

    
    [Fact]
    public async Task DeleteAudits()
    {
        
    }


    [Fact]
    public async Task UpdateAudits()
    {
        
    }

    public void Dispose()
    {
        (this.serviceProvider as IDisposable)?.Dispose();
    }
}

public class TestAuditInfoProvider : IAuditInfoProvider
{
    public string? AppLocation { get; set; } = "UNIT TESTS";
    public string? Tenant { get; set; } = "Test Tenant";
    public string? UserIdentifier { get; set; } = "Test User";
    public string? UserIpAddress { get; set; } = "0.0.0.0";
}