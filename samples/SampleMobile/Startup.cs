using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Navigation;
using Shiny;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace SampleMobile
{
    public class Startup : FrameworkStartup
    {
        public override void ConfigureApp(Application app, IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }


        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage");


        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            services.UsePush<MyPushDelegate>();
        }
    }
}
