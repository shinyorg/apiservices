using Shiny.Api.Push;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.AddConsole();
});

builder.Host.ConfigureServices(services =>
{
    services.AddControllersWithViews();
    //services.AddPushManagement(x => x)
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);
app.Run();