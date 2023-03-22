using Microsoft.Data.Sqlite;
using Sample;
using Shiny.Extensions.Push;

var builder = WebApplication.CreateBuilder(args);

if (!File.Exists("shiny.db"))
    File.Create("shiny.db");

//builder.Services.AddMail(XmlConfigurationExtensions =>)
//builder.Services.AddWebHooks();
var appleCfg = builder.Configuration.GetSection("Push:Apple");
var googleCfg = builder.Configuration.GetSection("Push:Google");
builder.Services.AddPushManagement(x => x
    .AddApple(new AppleConfiguration
    {
        AppBundleIdentifier = appleCfg["AppBundleIdentifier"]!,
        TeamId = appleCfg["TeamId"]!,
        Key = appleCfg["Key"]!,
        KeyId = appleCfg["KeyId"]!,
        IsProduction = false
        //JwtExpiryMinutes
    })
    .AddGoogleFirebase(new GoogleConfiguration
    {
        ServerKey = googleCfg["ServerKey"]!,
        SenderId = googleCfg["SenderId"]!,
        DefaultChannelId = googleCfg["DefaultChannelId"]!
    })
    .UseAdoNetRepository<SqliteConnection>(new DbRepositoryConfig(
        "Data Source=shiny.db",
        "@",
        "PushRegistrations",
        "PushRegistrationTags",
        true
    ))
    .AddShinyAndroidClickAction()
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapPushEndpoints();
app.MapAppEndpoints();

app.Run();