namespace Shiny.Extensions.Push;


public class Filter
{
    public string? DeviceToken { get; set; }
    public string? UserId { get; set; }
    public string[]? Platforms { get; set; }
    public string[]? Tags { get; set; }
}

