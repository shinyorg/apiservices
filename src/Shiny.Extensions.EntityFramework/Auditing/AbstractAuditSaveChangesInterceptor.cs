using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shiny.Auditing;


public abstract class AbstractAuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var entries = this.GetAuditEntries(eventData);

        var actualResult = base.SavedChanges(eventData, result);

        // TODO: post op
        // TODO: save to new dbcontext
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
        var auditInfo = this.GetAuditInfo();
        // TODO: I need to tag this on entities that are changing BEFORE change tracking?

        var changeTracker = eventData.Context!.ChangeTracker;
        changeTracker.DetectChanges();
        var entries = new List<AuditEntry>();


        foreach (var entry in changeTracker.Entries())
        {
            // Dot not audit entities that are not tracked, not changed, or not of type IAuditable
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || entry.Entity is not IAuditable)
                continue;

            var auditEntry = new AuditEntry
            {
                Operation = ToOperation(entry.State),
                EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue!.ToString()!,
                EntityType = entry.Metadata.ClrType.Name,
                Timestamp = DateTime.UtcNow,
                ChangeSet = this.CalculateChangeSet(entry)

                // TempProperties are properties that are only generated on save, e.g. ID's
                // These properties will be set correctly after the audited entity has been saved
                // TempProperties = entry.Properties.Where(p => p.IsTemporary).ToList(),
            };

            entries.Add(auditEntry);
        }

        return entries;
    }

    protected abstract AuditInfo GetAuditInfo();

    protected virtual Dictionary<string, object> CalculateChangeSet(EntityEntry entry)
    {
        var dict = new Dictionary<string, object>();
        foreach (var property in entry.Properties)
        {
            if (property.IsModified)
            {
                // don't track any binary or password - others?
                dict.Add(property.Metadata.Name, property.OriginalValue ?? "NULL");
            }
        }
        return dict;
    }
}