namespace SampleMauiPush;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp() => MauiApp
        .CreateBuilder()
        .UseMauiApp<App>()
        .UseMauiCommunityToolkit()
        .UseShinyFramework(
            new DryIocContainerExtension(),
            prism => prism.OnAppStart("NavigationPage/SettingsPage"),
            new(
                #if DEBUG
                ErrorAlertType.FullError
                #else
                ErrorAlertType.NoLocalize
                #endif
            )
        )
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); 
        })
        .RegisterInfrastructure()
        .RegisterViews()
        .Build();


    static MauiAppBuilder RegisterInfrastructure(this MauiAppBuilder builder)
    {
        builder.Configuration.AddJsonPlatformBundle();
#if DEBUG
        builder.Logging.AddConsole();
#endif
        var s = builder.Services;

        s.AddPush<SampleMauiPush.Delegates.MyPushDelegate>();
        s.AddDataAnnotationValidation();
        return builder;
    }


    static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        var s = builder.Services;

        s.RegisterForNavigation<SettingsPage, SettingsViewModel>();
        return builder;
    }
}
