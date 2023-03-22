using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shiny.Extensions.Push;


public record PushRegister(
    string Platform,
    string DeviceToken,
    string[]? Tags = null
);

public static class Extensions
{
    public static WebApplication MapPushEndpoints(this WebApplication app, string groupName = "push", bool requiresAuth = true, Func<HttpContext, string>? getUserId = null)
    {
        var group = app.MapGroup(groupName);

        if (requiresAuth)
            group.RequireAuthorization();

        group.MapPost(
            "/",
            async (
                [FromBody] PushRegister register,
                [FromServices] IPushManager push,
                HttpContext httpContext
            ) =>
            {
                var userId = getUserId?.Invoke(httpContext);
                await push.Register(new PushRegistration(
                    register.Platform,
                    register.DeviceToken,
                    register.Tags,
                    userId
                ));
            }
        );

        group.MapDelete(
            "/{platform}/{deviceToken}",
            async (
                [FromRoute] string platform,
                [FromRoute] string deviceToken,                
                [FromServices] IPushManager push
            ) => await push.UnRegister(platform, deviceToken)
        );

        return app;
    }
}