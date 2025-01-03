using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;

namespace Shiny.Modules;

public class RateLimitModule : IInfrastructureModule
{
    public void Add(WebApplicationBuilder builder)
    {

        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", x =>
            {
                x.PermitLimit = 3;
                x.Window = TimeSpan.FromSeconds(3);
                x.QueueLimit = 2;
            });
            // options.GeneralRules.Add(new RateLimitRule
            // {
            //     Endpoint = "*",
            //     Period = "1m",
            //     Limit = 100
            // });
        });
    }


    public void Use(WebApplication app) => app.UseRateLimiter();
}