using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Shiny.Extensions.Webhooks.Infrastructure;


public class DefaultHttpContentSerializer : IHttpContentSerializer
{
    public HttpContent Mutate(WebHookRegistration registration, object? args)
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

        return content;
    }



    protected virtual string ComputeHash(string salt, string content)
    {
        using var hasher = SHA256.Create();
        
        var bytes = Encoding.UTF8.GetBytes(content);
        var hashBytes = hasher.ComputeHash(bytes);
        return Encoding.UTF8.GetString(hashBytes);
    }
}
