using SampleWeb;
using Shiny.Api.Push;
using Shiny.Api.Push.Ef;
using Shiny.Api.Push.Providers;
using Shiny.Mail;
using Shiny.Mail.Impl;
using Shiny.Storage;
using Shiny.Storage.AzureBlobStorage;
using Shiny.Storage.FtpClient;
using Shiny.Storage.Impl;


var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(x => x.AddConsole());

builder.Host.ConfigureServices(services =>
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddControllersWithViews();

    services.AddPushManagement(x => x
        .AddApplePush(builder.Configuration.GetSection("Push:Apple").Get<AppleConfiguration>())
        .AddGoogle(builder.Configuration.GetSection("Push:Google").Get<GoogleConfiguration>())
        .UseEfRepository<SampleDbContext>()
    );

    services.AddMailProcessor(x => x
        .UseSmtpSender(builder.Configuration.GetSection("Mail:Smtp").Get<SmtpConfig>())
        .UseFileTemplateLoader("mailtemplates")
        //.UseSendGridSender(builder.Configuration["Mail:SendGridApiKey"])
        // mail processor, razor parser, and front matter parser loaded automatically
    );

    services.AddSingleton<IAsyncFileProvider, FileSystemAsyncFileProvider>();
    //services.AddSingleton<IAsyncFileProvider>(_ => new AzureBlobAsyncFileProvider())
    //services.AddSingleton<IAsyncFileProvider>(_ => new FtpAsyncFileProvider());
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);
app.UseSwagger();
app.UseSwaggerUI();

app.Run();