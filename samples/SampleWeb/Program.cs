using Microsoft.EntityFrameworkCore;
using SampleWeb;
using Shiny;
using Shiny.Extensions.Mail;
using Shiny.Extensions.Push;
using Shiny.Extensions.Localization;
using Shiny.Extensions.Push.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(x => {
    x.AddConsole();
    x.AddDebug();
});

builder.Host.ConfigureServices(services =>
{
    var cfg = builder.Configuration;

    services.AddSingleton(new LocalizationBuilder()
        .AddAssemblyResources(typeof(SampleDbContext).Assembly, true)
        .Build()
    );
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddControllersWithViews();

    services.AddDbContextFactory<SampleDbContext>(x =>
    {
        var connString = cfg.GetConnectionString("Main");
        x.UseSqlServer(connString);
        //x.LogTo(Console.Write);
    });

    services.AddPushManagement(x => x
        .AddApplePush(cfg.GetSection("Push:Apple").Get<AppleConfiguration>())
        .AddGooglePush(cfg.GetSection("Push:Google").Get<GoogleConfiguration>())
        .UseRepository<FilePushRepository>()
        //.UseEfRepository<SampleDbContext>()
    );

    services.AddMail(x => x
        .UseSmtpSender(cfg.GetSection("Mail:Smtp").Get<SmtpConfig>())
        .UseFileTemplateLoader("mailtemplates", "mailtemplate")
        //.UseSendGridSender(cfg["Mail:SendGridApiKey"])
        // mail processor, razor parser, and front matter parser loaded automatically
    );
});

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);
app.UseSwagger();
app.UseSwaggerUI();

app.Run();