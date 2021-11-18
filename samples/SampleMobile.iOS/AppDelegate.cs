using Foundation;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "SampleMobile.Startup",
    XamarinFormsAppTypeName = "SampleMobile.App"
)]
namespace SampleMobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
    }
}
