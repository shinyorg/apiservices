using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using System;
using System.Windows.Input;


namespace SampleMobile
{
    public class SettingsViewModel : ViewModel
    {
        public SettingsViewModel(AppSettings app, IDialogs dialogs)
        {
            this.ApiBaseUrl = app.ApiBaseUrl;

            this.Save = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var prevUrl = app.ApiBaseUrl;

                    try
                    {
                        await dialogs.LoadingTask(async () => {
                            app.ApiBaseUrl = this.ApiBaseUrl;
                            await app.ApiClient.GetFileProviders();
                        });
                        await dialogs.Snackbar("API URL updated");
                    }
                    catch
                    {
                        app.ApiBaseUrl = prevUrl;
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
