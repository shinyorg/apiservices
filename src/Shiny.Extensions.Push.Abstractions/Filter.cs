namespace Shiny.Extensions.Push;


public class Filter
{
    public string? DeviceToken { get; set; }
    public string? UserId { get; set; }
    public string? Platform { get; set; } // NULL FOR ALL
    public string[]? Tags { get; set; }
}

