using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shiny.Auditing;


// TODO: I need the entity ID after insert
// TODO: catch ExecuteUpdate & ExecuteDelete - how?  ExecuteDelete isn't something I believe in with audited tables anyhow - So only ExecuteUpdate
public class AuditSaveChangesInterceptor(IAuditInfoProvider provider) : SaveChangesInterceptor
{
    AuditScope? auditScope;
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        this.auditScope = AuditScope.Create(provider, eventData);   
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        this.auditScope = AuditScope.Create(provider, eventData);  
        return base.SavingChanges(eventData, result);
    }


    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (this.auditScope != null)
            await this.auditScope.Commit(cancellationToken);
        
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        this.auditScope?.Commit();
        return base.SavedChanges(eventData, result);
    }
}