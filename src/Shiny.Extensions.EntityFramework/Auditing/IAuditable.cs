namespace Shiny.Auditing;

public interface IAuditable
{
    string? LastEditUserIdentifier { get; set; }
    DateTimeOffset DateUpdated { get; set; }
    DateTimeOffset DateCreated { get; set; }
}