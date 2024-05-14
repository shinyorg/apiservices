using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Shiny.Auditing;


// TODO: I need the entity ID after insert
// TODO: catch ExecuteUpdate & ExecuteDelete - how?  ExecuteDelete isn't something I believe in with audited tables anyhow - So only ExecuteUpdate
public class AuditSaveChangesInterceptor(IAuditInfoProvider provider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var entries = this.GetAuditEntries(eventData);
        eventData.Context!.AddRange(entries);        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var entries = this.GetAuditEntries(eventData);
        eventData.Context!.AddRange(entries);        
        return base.SavingChanges(eventData, result);
    }
    

    static DbOperation ToOperation(EntityState state)
    {
        if (state == EntityState.Added)
            return DbOperation.Insert;

        if (state == EntityState.Deleted)
            return DbOperation.Delete;

        return DbOperation.Update;
    }
    

    protected virtual List<AuditEntry> GetAuditEntries(DbContextEventData eventData)
    {
        var entries = new List<AuditEntry>();
        var changeTracker = eventData.Context!.ChangeTracker;
        changeTracker.DetectChanges();

        foreach (var entry in changeTracker.Entries())
        {
            // Dot not audit entities that are not tracked, not changed, or not of type IAuditable
            if (entry.State != EntityState.Detached && 
                entry.State != EntityState.Unchanged &&
                entry.Entity is IAuditable auditable)
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
                    EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue!.ToString()!,
                    EntityType = entry.Metadata.ClrType.Name,
                    Timestamp = DateTime.UtcNow,
                    ChangeSet = this.CalculateChangeSet(entry), // TODO: NULL on add
                    
                    UserIdentifier = provider.UserIdentifier,
                    UserIpAddress = provider.UserIpAddress,
                    Tenant = provider.Tenant,
                    AppLocation = provider.AppLocation
                };
                entries.Add(auditEntry);
            }
        }
        return entries;
    }
    

    protected virtual JsonDocument CalculateChangeSet(EntityEntry entry)
    {
        // TODO: if I'm deleting, I want all the original values (even ignored?)
        var dict = new Dictionary<string, object>();
        foreach (var property in entry.Properties)
        {
            if (this.IsAuditedProperty(property))
            {
                dict.Add(property.Metadata.Name, property.OriginalValue ?? "NULL");
            }
        }

        var json = JsonSerializer.SerializeToDocument(dict);
        return json;
    }


    protected virtual bool IsAuditedProperty(PropertyEntry entry)
    {
        if (!entry.IsModified)
            return false;

        if (entry.OriginalValue is byte[])
            return false;

        if (this.IsPropertyIgnored(entry.Metadata.Name))
            return false;
        
        return true;
    }

    
    protected virtual bool IsPropertyIgnored(string propertyName) => propertyName switch
    {
        nameof(IAuditable.LastEditUserIdentifier) => true,
        nameof(IAuditable.DateCreated) => true,
        nameof(IAuditable.DateUpdated) => true,
        _ => false
    };
}