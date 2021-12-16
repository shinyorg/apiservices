using Microsoft.EntityFrameworkCore;
using SampleWeb;
using Shiny.Storage;
using Shiny.Storage.AzureBlobStorage;
using Shiny.Storage.FtpClient;
using Shiny.Storage.Impl;
using Shiny;
using Shiny.Extensions.Mail;
using Shiny.Extensions.Push;


var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(x => {
    x.AddConsole();
    x.AddDebug();
});

builder.Host.ConfigureServices(services =>
{
    var cfg = builder.Configuration;

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
        .UseEfRepository<SampleDbContext>()
    );

    services.AddMailProcessor(x => x
        .UseSmtpSender(cfg.GetSection("Mail:Smtp").Get<SmtpConfig>())
        .UseFileTemplateLoader("mailtemplates", "mailtemplate")
        //.UseSendGridSender(cfg["Mail:SendGridApiKey"])
        // mail processor, razor parser, and front matter parser loaded automatically
    );

    services.AddSingleton<IAsyncFileProvider, FileSystemAsyncFileProvider>();
    //services.AddSingleton<IAsyncFileProvider>(_ => new AzureBlobAsyncFileProvider())
    //services.AddSingleton<IAsyncFileProvider>(_ => new FtpAsyncFileProvider());
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