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
            this.ApiBaseUri = settings.ApiBaseUrl;

            this.Save = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var prevUrl = settings.ApiBaseUrl;

                    try
                    {
                        await dialogs.LoadingTask(async () => {
                            settings.ApiBaseUrl = this.ApiBaseUri;
                            await api.GetFileProviders();
                        });
                    }
                    catch
                    {
                        settings.ApiBaseUrl = prevUrl;
                        throw;
                    }
                },
                this.WhenAny(
                    x => x.ApiBaseUri,
                    x => !x.GetValue().IsEmpty() && Uri.TryCreate(x.GetValue(), UriKind.Absolute, out _)
                )
            );
        }


        public ICommand Save { get; }
        [Reactive] public string ApiBaseUri { get; set; }
    }
}
