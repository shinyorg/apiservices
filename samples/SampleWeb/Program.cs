using SampleWeb;
using Shiny.Api.Push;
using Shiny.Api.Push.Ef;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.AddConsole();
});

builder.Host.ConfigureServices(services =>
{
    services.AddControllersWithViews();
    services.AddPushManagement(x => x
        //.AddApple()
        //.AddGoogle()
        .UseEfRepository<SampleDbContext>()
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