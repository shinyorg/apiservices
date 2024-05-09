using Microsoft.EntityFrameworkCore;

namespace Shiny.Auditing;

public interface IAuditable
{
    AuditInfo Audit { get; set; }
}

[Owned]
public class AuditInfo
{
    public int? UserId { get; set; }
    public string? Tenant { get; set; }
    public string? Url { get; set; }
    public string? IpAddress { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}