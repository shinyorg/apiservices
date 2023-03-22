using Prism.DryIoc;

namespace SampleMauiPush;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp() => MauiApp
        .CreateBuilder()
        .UseMauiApp<App>()
        .UseMauiCommunityToolkit()
        .UseShinyFramework(
            new DryIocContainerExtension(),
            prism => prism.OnAppStart("NavigationPage/MainPage")
        )
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); 
        })
        .RegisterInfrastructure()
        .RegisterAppServices()
        .RegisterViews()
        .Build();


    static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder) 
    {
        // register your own services here!
        return builder;
    }


    static MauiAppBuilder RegisterInfrastructure(this MauiAppBuilder builder)
    {
        builder.Configuration.AddJsonPlatformBundle();
#if DEBUG
        builder.Logging.AddConsole();
#endif
        var s = builder.Services;
        s.AddPush<SampleMauiPush.Delegates.MyPushDelegate>();
        s.AddDataAnnotationValidation();
        s.AddGlobalCommandExceptionHandler(new(
#if DEBUG
            ErrorAlertType.FullError
#else
            ErrorAlertType.NoLocalize
#endif
        ));
        return builder;
    }


    static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        var s = builder.Services;


        s.RegisterForNavigation<MainPage, MainViewModel>();
        return builder;
    }
}
