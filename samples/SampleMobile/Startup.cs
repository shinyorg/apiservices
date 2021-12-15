using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Navigation;
using SampleMobile.Mail;
using SampleMobile.Push;
using SampleMobile.Storage;
using Shiny;
using System;
using System.Net.Http;
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
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();

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
            var tab4 = start + nameof(SettingsPage);
            return navigator.Navigate($"TabbedPage?{tab1}&{tab2}&{tab3}&{tab4}");
        }


        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            services.AddSingleton<AppSettings>();
            services.UsePush<MyPushDelegate>();
            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<AppSettings>();
                var httpClient = new HttpClient();
                settings
                    .WhenAnyProperty(x => x.ApiBaseUrl)
                    .Subscribe(x => httpClient.BaseAddress = new Uri(x));
                return Refit.RestService.For<ISampleApi>(httpClient);
            });

            services.UseXfMaterialDialogs();
            services.UseGlobalCommandExceptionHandler(x => x.AlertType = ErrorAlertType.FullError);
        }
    }
}
