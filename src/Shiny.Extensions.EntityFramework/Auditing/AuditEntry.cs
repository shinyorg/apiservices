namespace Shiny.Auditing;

public class AuditEntry
{
    public int Id { get; set; }
    public string EntityId { get; set; }
    public string EntityType { get; set; }

    public AuditInfo Info { get; set; }
    public DbOperation Operation { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public Dictionary<string, object> ChangeSet { get; set; } // TODO: from current main record
}

public enum DbOperation
{
    Insert,
    Update,
    Delete
}