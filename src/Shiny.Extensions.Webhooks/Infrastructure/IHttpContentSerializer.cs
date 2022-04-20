namespace Shiny.Extensions.Webhooks.Infrastructure
{
    public interface IHttpContentSerializer
    {
        HttpContent Mutate(WebHookRegistration registration, object? args);
    }
}
