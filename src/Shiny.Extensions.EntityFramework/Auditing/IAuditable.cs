using Microsoft.EntityFrameworkCore;

namespace Shiny.Auditing;

public interface IAuditable
{
    string? LastEditUserIdentifier { get; set; }
    DateTimeOffset DateCreated { get; set; }
}

[Owned]
public class AuditInfo
{
    public string? UserIdentifier { get; set; }
    public string? Url { get; set; }
    public string? IpAddress { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}