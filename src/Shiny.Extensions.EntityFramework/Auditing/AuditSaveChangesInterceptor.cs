using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;

namespace Shiny.Auditing;


public class AuditSaveChangesInterceptor(IAuditInfoProvider provider) : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var entries = this.GetAuditEntries(eventData);
        eventData.Context!.AddRange(entries);
        
        var actualResult = base.SavedChanges(eventData, result);
        return actualResult;
    }


    static DbOperation ToOperation(EntityState state)
    {
        if (state == EntityState.Added)
            return DbOperation.Insert;

        if (state == EntityState.Deleted)
            return DbOperation.Delete;

        return DbOperation.Update;
    }


    protected virtual List<AuditEntry> GetAuditEntries(SaveChangesCompletedEventData eventData)
    {
        var entries = new List<AuditEntry>();
        var auditInfo = provider.GetAuditInfo();
        var changeTracker = eventData.Context!.ChangeTracker;
        changeTracker.DetectChanges();

        foreach (var entry in changeTracker.Entries())
        {
            // Dot not audit entities that are not tracked, not changed, or not of type IAuditable
            if (entry.State != EntityState.Detached && 
                entry.State != EntityState.Unchanged &&
                entry.Entity is IAuditable auditable)
            {
                auditable.LastEditUserIdentifier = auditInfo.UserIdentifier;
                if (auditable.DateCreated == DateTimeOffset.MinValue)
                    auditable.DateCreated = DateTimeOffset.UtcNow;

                entry.DetectChanges();
                var auditEntry = new AuditEntry
                {
                    Operation = ToOperation(entry.State),
                    EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue!.ToString()!,
                    EntityType = entry.Metadata.ClrType.Name,
                    Timestamp = DateTime.UtcNow,
                    ChangeSet = this.CalculateChangeSet(entry),
                    Info = auditInfo
                };
                entries.Add(auditEntry);
            }
        }
        return entries;
    }
    

    protected virtual Dictionary<string, object> CalculateChangeSet(EntityEntry entry)
    {
        var dict = new Dictionary<string, object>();
        foreach (var property in entry.Properties)
        {
            if (this.IsAuditedProperty(property))
            {
                dict.Add(property.Metadata.Name, property.OriginalValue ?? "NULL");
            }
        }
        return dict;
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
        nameof(IAuditable.LastEditUserIdentifier) => false,
        nameof(IAuditable.DateCreated) => false,
        _ => true
    };
}