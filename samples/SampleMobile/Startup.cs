using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Navigation;
using SampleMobile.Mail;
using SampleMobile.Push;
using SampleMobile.Storage;
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
            containerRegistry.RegisterForNavigation<MainPage>();

            containerRegistry.RegisterForNavigation<CreateMailPage, CreateMailViewModel>();
            containerRegistry.RegisterForNavigation<ExplorerPage, ExplorerViewModel>();
            containerRegistry.RegisterForNavigation<CreatePushPage, CreatePushViewModel>();
        }


        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage");


        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            services.UsePush<MyPushDelegate>();
            services.AddSingleton(Refit.RestService.For<ISampleApi>("https://acrmonster"));
        }
    }
}
