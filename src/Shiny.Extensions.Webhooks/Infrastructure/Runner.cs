using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Extensions.Webhooks.Infrastructure;


public class Runner : IRunner
{
    public static HttpClient? TestHttpClient { get; set; }
    readonly HttpClient httpClient = TestHttpClient ?? new();
    readonly IHttpContentSerializer httpContentManager;

    public Runner(IHttpContentSerializer httpContentManager) 
        => this.httpContentManager = httpContentManager ?? throw new ArgumentNullException(nameof(httpContentManager));


    public async Task<T?> Request<T>(WebHookRegistration registration, object? args, CancellationToken cancelToken)
    {
        var response = await this.SendHttp(registration, args, cancelToken).ConfigureAwait(false);
        var result = await response.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);
        if (String.IsNullOrWhiteSpace(result))
            return default;

        var obj = JsonSerializer.Deserialize<T>(result);
        return obj;
    }


    public Task Send(WebHookRegistration registration, object? args, CancellationToken cancelToken)
        => this.SendHttp(registration, args, cancelToken);


    protected virtual async Task<HttpResponseMessage> SendHttp(WebHookRegistration registration, object? args, CancellationToken cancelToken)
    {
        var content = this.httpContentManager.Mutate(registration, args);

        var response = await this.httpClient
            .PostAsync(
                registration.CallbackUri,
                content,
                cancelToken
            )
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return response;
    }
}
