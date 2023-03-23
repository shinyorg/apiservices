using System.Security.Claims;
using Microsoft.Data.Sqlite;
using Sample;
using Shiny.Extensions.Mail;
using Shiny.Extensions.Push;

var builder = WebApplication.CreateBuilder(args);

if (!File.Exists("shiny.db"))
    File.Create("shiny.db");

//builder.Services.AddMail(XmlConfigurationExtensions =>)
//builder.Services.AddWebHooks();
builder.Services.AddMail(mail =>
{
    var cfg = builder.Configuration.GetSection("Mail");
    mail
        .UseSmtpSender(new SmtpConfig
        {
            EnableSsl = cfg.GetValue("EnableSsl", true),
            Host = cfg["Host"],
            Port = cfg.GetValue("Port", 587)
        })
        //.UseSendGridSender("SendGridApiKey")
        //.UseFileTemplateLoader("File Path to templates")
        .UseAdoNetTemplateLoader<SqliteConnection>(
            "Data Source=shiny.db",
            "@",
            "MailTemplates",
            true
        );
});

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
app.MapPushEndpoints("push", true, x => x.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
app.MapAppEndpoints();

app.Run();