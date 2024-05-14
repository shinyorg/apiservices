using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shiny.Auditing;

namespace Shiny.Extensions.EntityFramework.Tests;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<AuditEntry> AuditEntries => this.Set<AuditEntry>();
    public DbSet<Manufacturer> Manufacturers => this.Set<Manufacturer>();
    public DbSet<Model> Models => this.Set<Model>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetDefaultStringLength();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapEasyPropertyIds();
    }
}


public class Manufacturer : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string? LastEditUserIdentifier { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    
    public ICollection<Model> Models { get; set; }
}

public class Model : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public JsonDocument? Properties { get; set; }
    
    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; }
    
    public string? LastEditUserIdentifier { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}