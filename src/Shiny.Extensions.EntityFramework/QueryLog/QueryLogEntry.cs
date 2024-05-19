namespace Shiny.QueryLog;

public class QueryLogEntry
{
    public int Id { get; set; }
    public string Query { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    
    public string? UserIdentifier { get; set; }
    public string? AppLocation { get; set; }
    public string? UserIpAddress { get; set; }
}