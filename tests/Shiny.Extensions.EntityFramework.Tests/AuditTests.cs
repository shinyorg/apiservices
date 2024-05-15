using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shiny.Auditing;

namespace Shiny.Extensions.EntityFramework.Tests;


// TODO: check entityId & entityType
public class AuditTests : IDisposable
{
    readonly TestAuditInfoProvider auditProvider;
    readonly IServiceProvider serviceProvider;

    public AuditTests()
    {
        this.auditProvider = new();
        
        var services = new ServiceCollection();
        services.AddSingleton<IAuditInfoProvider>(this.auditProvider);
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddDbContext<TestDbContext>((sp, opts) =>
        {
            // .UseSqlite("Data Source=test.db")
            opts.UseNpgsql(new NpgsqlDataSourceBuilder("User ID=sa;Password=Blargh911!;Host=localhost;Port=5432;Database=AuditUnitTests;Pooling=true;Connection Lifetime=30;").Build());
            var interceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();
            opts.AddInterceptors(interceptor);
        });
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
        await this.Seed();

        await this.DoDb(async data =>
        {
            var audits = await data.AuditEntries.ToListAsync();
            audits.Count.Should().Be(2);
            audits.ForEach(x => AssertAudit(x, DbOperation.Insert));

            var manu = await data.Manufacturers.FirstAsync()!;
            manu.LastEditUserIdentifier.Should().Be("Test User");
            manu.DateUpdated.Date.Should().Be(DateTimeOffset.UtcNow.Date, "DateUpdated is wrong");
            manu.DateCreated.Date.Should().Be(DateTimeOffset.UtcNow.Date, "DateCreated is wrong");
        });
    }

    
    [Fact]
    public async Task DeleteAudits()
    {
        await this.Seed();
        await this.DoDb(async data =>
        {
            var manu = await data.Manufacturers.FirstAsync();
            data.Remove(manu);
            await data.SaveChangesAsync();
        });
        await this.DoDb(async data =>
        {
            var audit = await data.AuditEntries.FirstOrDefaultAsync(x => x.Operation == DbOperation.Delete);
            audit.Should().NotBeNull("No Delete Audit Found");
            AssertAudit(audit!, DbOperation.Delete);

            audit!.ChangeSet.RootElement.GetProperty("Name").GetString().Should().Be("Cadillac");
        });
    }


    [Fact]
    public async Task UpdateAudits()
    {
        await this.Seed();
        await this.DoDb(async data =>
        {
            var manu = await data.Manufacturers.FirstAsync();
            manu.Name = "UPDATE";
            await data.SaveChangesAsync();
        });
        await this.DoDb(async data =>
        {
            var audit = await data.AuditEntries.FirstOrDefaultAsync(x => x.Operation == DbOperation.Update);
            audit.Should().NotBeNull("No Delete Audit Found");
            AssertAudit(audit!, DbOperation.Update);
            
            audit!.ChangeSet.RootElement.GetProperty("Name").GetString().Should().Be("Cadillac");
        });        
    }


    Task Seed() => this.DoDb(data =>
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
    
    
    void AssertAudit(AuditEntry audit, DbOperation op)
    {
        audit.Operation.Should().Be(op, "Invalid Operation");
        audit.UserIdentifier.Should().Be("Test User");
        audit.UserIpAddress.Should().Be("0.0.0.0");
        audit.AppLocation.Should().Be("UNIT TESTS");
    }

    public void Dispose() => (this.serviceProvider as IDisposable)?.Dispose();
}

public class TestAuditInfoProvider : IAuditInfoProvider
{
    public string? AppLocation { get; set; } = "UNIT TESTS";
    public string? Tenant { get; set; } = "Test Tenant";
    public string? UserIdentifier { get; set; } = "Test User";
    public string? UserIpAddress { get; set; } = "0.0.0.0";
}