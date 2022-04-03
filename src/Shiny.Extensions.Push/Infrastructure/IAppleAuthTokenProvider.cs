namespace Shiny.Extensions.Push.Infrastructure
{
    public interface IAppleAuthTokenProvider
    {
        string GetAuthToken(AppleConfiguration config);
    }
}
