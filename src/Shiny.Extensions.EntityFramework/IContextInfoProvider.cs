namespace Shiny;

public interface IContextInfoProvider
{
    /// <summary>
    /// Can be a URL or anything else if available
    /// </summary>
    string? AppLocation { get; }
    
    /// <summary>
    /// Your user ID or name if available
    /// </summary>
    string? UserIdentifier { get; }
    
    /// <summary>
    /// The IP address of the remote user if available
    /// </summary>
    string? UserIpAddress { get; }
    
    
    // IDictionary<string, object> AdditionalProperties { get; }
}