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
            containerRegistry.RegisterForNavigation<TabbedPage>();

            containerRegistry.RegisterForNavigation<CreateMailPage, CreateMailViewModel>();
            containerRegistry.RegisterForNavigation<RegistrationListPage, RegistrationListViewModel>();
            containerRegistry.RegisterForNavigation<ExplorerPage, ExplorerViewModel>();
            containerRegistry.RegisterForNavigation<CreatePushPage, CreatePushViewModel>();
        }


        public override Task RunApp(INavigationService navigator)
        { 
            var start = $"{KnownNavigationParameters.CreateTab}={nameof(NavigationPage)}|";
            var tab1 = start + nameof(CreatePushPage);
            var tab2 = start + nameof(ExplorerPage);
            var tab3 = start + nameof(CreateMailPage);
            return navigator.Navigate($"TabbedPage?{tab1}&{tab2}&{tab3}");
        }


        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            services.UsePush<MyPushDelegate>();
            services.AddSingleton(Refit.RestService.For<ISampleApi>("https://acrmonster"));

            services.UseXfMaterialDialogs();
            services.UseGlobalCommandExceptionHandler();
        }
    }
}
