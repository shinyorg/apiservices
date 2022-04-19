namespace Shiny.Extensions.Webhooks.Infrastructure;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


public class Runner : IRunner
{
    public static HttpClient? TestHttpClient { get; set; }
    readonly HttpClient httpClient = TestHttpClient ?? new();


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
        var jsonString = String.Empty;

        if (args is string s)
            jsonString = s;
        else if (args != null)
            jsonString = JsonSerializer.Serialize(args);

        var content = new StringContent(
            jsonString,
            Encoding.UTF8,
            "application/json"
        );

        //https://stripe.com/docs/webhooks/signatures#compare-signatures
        if (!String.IsNullOrWhiteSpace(registration.HashVerification))
        {
            var hash = this.ComputeHash(registration.HashVerification, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
            content.Headers.TryAddWithoutValidation("signed_hash", hash);
        }
        
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


    protected virtual string ComputeHash(string salt, string content)
    {
        using (var hasher = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var hashBytes = hasher.ComputeHash(bytes);
            return Encoding.UTF8.GetString(hashBytes);
        }
    }
}
