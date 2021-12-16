using Android.App;
using Android.Content.PM;
using Xamarin.Forms.Platform.Android;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "SampleMobile.Startup",
    XamarinFormsAppTypeName = "SampleMobile.App"
)]

namespace SampleMobile.Droid
{
    [Activity(
        Label = "API Services",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges =
            ConfigChanges.ScreenSize |
            ConfigChanges.Orientation |
            ConfigChanges.UiMode |
            ConfigChanges.ScreenLayout |
            ConfigChanges.SmallestScreenSize
    )]
    public partial class MainActivity : FormsAppCompatActivity
    {
    }
}