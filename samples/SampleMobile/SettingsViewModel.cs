using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using System;
using System.Windows.Input; 


namespace SampleMobile
{
    public class SettingsViewModel : ViewModel
    {
        public SettingsViewModel(ISampleApi api,
                                 AppSettings settings,
                                 IDialogs dialogs)
        {
            this.ApiBaseUrl = settings.ApiBaseUrl;

            this.Save = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var prevUrl = settings.ApiBaseUrl;

                    try
                    {
                        await dialogs.LoadingTask(async () => {
                            settings.ApiBaseUrl = this.ApiBaseUrl;
                            await api.GetFileProviders();
                        });
                        await dialogs.Snackbar("Swipe away the app to restart it");
                    }
                    catch
                    {
                        settings.ApiBaseUrl = prevUrl;
                        throw;
                    }
                },
                this.WhenAny(
                    x => x.ApiBaseUrl,
                    x => !x.GetValue().IsEmpty() && Uri.TryCreate(x.GetValue(), UriKind.Absolute, out _)
                )
            );
        }


        public ICommand Save { get; }
        [Reactive] public string ApiBaseUrl { get; set; }
    }
}
