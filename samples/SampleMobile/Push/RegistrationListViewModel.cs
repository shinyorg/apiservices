using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SampleWeb.Contracts;
using Shiny;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SampleMobile.Push
{
    public class RegistrationListViewModel : ViewModel
    {
        Registration? filter;


        public RegistrationListViewModel(ISampleApi apiClient, IDialogs dialogs)
        {
            this.Load = this.LoadingCommand(async () =>
            {
                this.Registrations = await apiClient.GetRegistrations(this.filter!);
            });
            this.WhenAnyValue(x => x.SelectedReg)
                .Skip(1)
                .SubscribeAsync(async x =>
                {
                    var result = await dialogs.Confirm("Remove this registration?");
                    if (result)
                    {
                        try
                        {
                            await apiClient.UnRegister(x);
                            this.Load.Execute(null);
                            await dialogs.Snackbar("Successfully removed registration");
                        }
                        catch (Exception ex)
                        {
                            await dialogs.Alert(ex.ToString());
                        }
                    }
                })
                .DisposedBy(this.DestroyWith);
        }


        public ICommand Load { get; }
        [Reactive] public Registration SelectedReg { get; set; }
        [Reactive] public List<Registration> Registrations { get; private set; }


        public override Task InitializeAsync(INavigationParameters parameters)
        {
            this.filter = parameters.GetValue<Registration>("Filter");
            this.Load.Execute(null);
            return Task.CompletedTask;
        }
    }
}
