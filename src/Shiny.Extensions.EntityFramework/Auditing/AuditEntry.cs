using System.Text.Json;

namespace Shiny.Auditing;

public class AuditEntry
{
    public int Id { get; set; }
    public string EntityId { get; set; }
    public string TableName { get; set; }

    public string? UserIdentifier { get; set; }
    public string? AppLocation { get; set; }
    public string? UserIpAddress { get; set; }
    
    public DbOperation Operation { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public JsonDocument ChangeSet { get; set; }
    // public Dictionary<string, object> ChangeSet { get; set; }
}

public enum DbOperation
{
    Insert = 1,
    Update = 2,
    Delete = 3
}