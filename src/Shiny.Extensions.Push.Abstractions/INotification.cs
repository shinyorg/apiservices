namespace Shiny.Extensions.Push;


public interface INotification
{
    string? Title { get; set; }
    string? Message { get; set; }
    string? CategoryOrChannel { get; set; }
    string? ImageUri { get; set; }
    TimeSpan? Expiration { get; set; }
    Dictionary<string, string>? Data { get; set; }
}