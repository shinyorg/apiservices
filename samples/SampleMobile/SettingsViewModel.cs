using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Shiny.Extensions.Dialogs;

using System;
using System.Windows.Input;


namespace SampleMobile
{
    public class SettingsViewModel : ViewModel
    {
        public SettingsViewModel(AppSettings app)
        {
            this.ApiBaseUrl = app.ApiBaseUrl;

            this.Save = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var prevUrl = app.ApiBaseUrl;

                    try
                    {
                        await this.Dialogs.LoadingTask(async () => {
                            app.ApiBaseUrl = this.ApiBaseUrl;
                            await app.ApiClient.GetFileProviders();
                        });
                        await this.Dialogs.Snackbar("API URL updated");
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
