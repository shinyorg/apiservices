using Microsoft.Data.SqlClient;
using Shiny.Extensions.Push;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddPushManagement(x => x
    .AddApple(new AppleConfiguration
    {
        AppBundleIdentifier = "",
        TeamId = "",
        Key = "",
        KeyId = "",
        IsProduction = false
        //JwtExpiryMinutes
    })
    .AddGoogleFirebase(new GoogleConfiguration
    {
        ServerKey = "",
        SenderId = "",
        DefaultChannelId = ""
    })
    .UseAdoNetRepository<SqlConnection>(new DbRepositoryConfig(
        builder.Configuration.GetConnectionString("Main")!,
        "@",
        "PushRegistrations",
        "PushRegistrationTags",
        true
    ))
    .AddShinyAndroidClickAction()
);

var app = builder.Build();
app.UseHttpsRedirection();
app.Run();