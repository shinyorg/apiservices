using System;


namespace Shiny.Extensions.Push.Infrastructure
{
    public record JwtToken(string Value, DateTime Expires);
}
