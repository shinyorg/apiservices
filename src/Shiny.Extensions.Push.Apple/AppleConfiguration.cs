namespace Shiny.Extensions.Push;


public class AppleConfiguration
{
    public int JwtExpiryMinutes { get; set; } = 50;
    public string Key { get; set; }
    public string KeyId { get; set; }
    public string TeamId { get; set; }
    public string AppBundleIdentifier { get; set; }
    public bool IsProduction { get; set; }


    public void AssertValid()
    {
        if (this.JwtExpiryMinutes < 20 || this.JwtExpiryMinutes > 60)
            throw new InvalidOperationException("JwtExpiryMinutes must be between 20-60 minutes");

        ArgumentNullException.ThrowIfNull(this.TeamId);
        ArgumentNullException.ThrowIfNull(this.Key);
        ArgumentNullException.ThrowIfNull(this.KeyId);
        ArgumentNullException.ThrowIfNull(this.AppBundleIdentifier);
    }
}

