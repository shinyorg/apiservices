namespace Shiny.Extensions.Webhooks.Impl;

using System.Text;

public class WebHookRunner : IWebHookRunner
{
    readonly HttpClient httpClient = new HttpClient();


    public async Task<T> RequestWithResponse<T>(WebHookRegistration registration, object args, CancellationToken cancelToken)
    {
        var response = await this.SendHttp(registration, args, cancelToken).ConfigureAwait(false);
        var result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
        // TODO: json

        return default(T);
    }


    public async Task Send(string eventName, object args)
    {
        var registrations = new List<WebHookRegistration>();

        await Parallel.ForEachAsync(
            registrations,
            new ParallelOptions { MaxDegreeOfParallelism = 3 }, // TODO: config
            async (reg, token) =>
            {
                await this.SendHttp(reg, args, token).ConfigureAwait(false);
            }
        );
    }


    protected virtual async Task<HttpResponseMessage> SendHttp(WebHookRegistration registration, object args, CancellationToken cancelToken)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(registration.ExecutionTimeoutSeconds));
        using var cancelSub = cancelToken.Register(() => timeout.Cancel());

        var content = new StringContent(
            "",
            Encoding.UTF8,
            "application/json"
        );
        var response = await this.httpClient
            .PostAsync(
                registration.CallbackUri,
                content,
                timeout.Token
            )
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return response;
    }
}
