using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shiny.Auditing;

public class AuditScope
{
    readonly List<EntityAuditContext> entries;
    readonly DbContext data;
    
    
    AuditScope(DbContext data, List<EntityAuditContext> entries)
    {
        this.data = data;
        this.entries = entries;
    }
    
    
    public static AuditScope Create(IAuditInfoProvider provider, DbContextEventData eventData)
    {
        var state = AuditScope.BuildState(provider, eventData);
        var scope = new AuditScope(eventData.Context!, state);
        return scope;
    }


    public async ValueTask Commit(CancellationToken cancellationToken)
    {
        if (this.entries.Count == 0)
            return;
        
        this.CompleteAudit();
        await this.data.SaveChangesAsync(cancellationToken);
    }


    public void Commit()
    {
        if (this.entries.Count == 0)
            return;
        
        this.CompleteAudit();
        this.data.SaveChanges();
    }
    

    void CompleteAudit()
    {
        foreach (var entry in this.entries)
        {
            entry.CurrentAudit.EntityId = GetPrimaryKey(entry.Entry);
            this.data.Add(entry.CurrentAudit);
        }
    }


    static string GetPrimaryKey(EntityEntry entry)
    {
        var meta = entry.Properties.Where(x => x.Metadata.IsPrimaryKey()).ToList();
        if (meta.Count == 1)
            return meta.First().CurrentValue!.ToString()!;

        var primaryKeys = new string[meta.Count];
        for (var i = 0; i < meta.Count; i++)
        {
            var key = meta[i];
            primaryKeys[i] = $"{key.Metadata.Name}-{key.CurrentValue!}";
        }

        var result = String.Join('_', primaryKeys);
        return result;
    }
    
    static DbOperation ToOperation(EntityState state)
    {
        if (state == EntityState.Added)
            return DbOperation.Insert;

        if (state == EntityState.Deleted)
            return DbOperation.Delete;

        return DbOperation.Update;
    }
    
    
    static List<EntityAuditContext> BuildState(IAuditInfoProvider provider, DbContextEventData eventData)
    {
        var entries = new List<EntityAuditContext>();
        var changeTracker = eventData.Context!.ChangeTracker;
        changeTracker.DetectChanges();

        foreach (var entry in changeTracker.Entries())
        {
            if (entry.State != EntityState.Detached && 
                entry.State != EntityState.Unchanged &&
                entry.Entity is IAuditable)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues[nameof(IAuditable.DateUpdated)] = DateTimeOffset.UtcNow;
                }
                else if (entry.State == EntityState.Added)
                {
                    entry.CurrentValues[nameof(IAuditable.DateUpdated)] = DateTimeOffset.UtcNow;
                    entry.CurrentValues[nameof(IAuditable.DateCreated)] = DateTimeOffset.UtcNow;
                }
                entry.CurrentValues[nameof(IAuditable.LastEditUserIdentifier)] = provider.UserIdentifier;
                
                var auditEntry = new AuditEntry
                {
                    Operation = ToOperation(entry.State),
                    TableName = entry.Metadata.GetTableName()!,
                    Timestamp = DateTime.UtcNow,
                    ChangeSet = CalculateChangeSet(entry), // what about post values?
                    
                    UserIdentifier = provider.UserIdentifier,
                    UserIpAddress = provider.UserIpAddress,
                    AppLocation = provider.AppLocation
                };
                entries.Add(new EntityAuditContext(entry, auditEntry));
            }
        }
        return entries;
    }
    

    static JsonDocument CalculateChangeSet(EntityEntry entry)
    {
        var dict = new Dictionary<string, object>();
        foreach (var property in entry.Properties)
        {
            if (IsAuditedProperty(property) && (entry.State == EntityState.Deleted || property.IsModified))
            {
                dict.Add(property.Metadata.Name, property.OriginalValue ?? "NULL");
            }
        }

        var json = JsonSerializer.SerializeToDocument(dict);
        return json;
    }


    static bool IsAuditedProperty(PropertyEntry entry)
    {
        if (entry.OriginalValue is byte[])
            return false;

        if (IsPropertyIgnored(entry.Metadata.Name))
            return false;
        
        return true;
    }

    
    static bool IsPropertyIgnored(string propertyName) => propertyName switch
    {
        nameof(IAuditable.LastEditUserIdentifier) => true,
        nameof(IAuditable.DateCreated) => true,
        nameof(IAuditable.DateUpdated) => true,
        _ => false
    };    
}

public record EntityAuditContext(
    EntityEntry Entry,
    AuditEntry CurrentAudit
);