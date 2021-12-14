using SampleWeb;
using Shiny.Api.Push;
using Shiny.Api.Push.Ef;
using Shiny.Api.Push.Providers;
using Shiny.Mail;
using Shiny.Mail.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.AddConsole();
});

builder.Host.ConfigureServices(services =>
{
    services.AddControllersWithViews();
    services.AddPushManagement(x => x
        .AddApplePush(builder.Configuration.GetSection("Push:Apple").Get<AppleConfiguration>())
        .AddGoogle(builder.Configuration.GetSection("Push:Google").Get<GoogleConfiguration>())
        .UseEfRepository<SampleDbContext>()
    );

    services.AddMailProcessor(x => x
        .UseSmtpSender(builder.Configuration.GetSection("Mail:Smtp").Get<SmtpConfig>())
        .UseFileTemplateLoader("mail")
        // mail processor, razor parser, and front matter parser loaded automatically
    );
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);
app.Run();