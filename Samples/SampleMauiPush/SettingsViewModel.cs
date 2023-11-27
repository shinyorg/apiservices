using Shiny.Push;

namespace SampleMauiPush;


public class SettingsViewModel : ViewModel
{
    readonly IPushManager pushManager;


    public SettingsViewModel(
        BaseServices services,
        IPushManager pushManager
    ) : base(services)
    {
        this.pushManager = pushManager;

        //        this.RequestAccess = this.LoadingCommand(async () =>
        //        {
        //            var result = await this.pushManager.RequestAccess();
        //            this.AccessStatus = result.Status;
        //            this.Refresh();
        //#if NATIVE
        //                if (this.AccessStatus == AccessState.Available)
        //                    await this.Try(() => this.apiClient.Register(result.RegistrationToken!));
        //#endif
        //        });

        //        this.UnRegister = this.LoadingCommand(async () =>
        //        {
        //            var deviceToken = this.pushManager.CurrentRegistrationToken;
        //            await this.pushManager.UnRegister();
        //            this.AccessStatus = AccessState.Disabled;
        //            this.Refresh();
        //#if NATIVE
        //                await this.Try(() => this.apiClient.UnRegister(deviceToken!));
        //#endif
        //        });

        //        this.ResetBaseUri = new Command(() =>
        //        {
        //            this.apiClient.Reset();
        //            this.RaisePropertyChanged(nameof(this.BaseUri));
        //        });
    }


    public ICommand RequestAccess { get; }
    public ICommand UnRegister { get; }
    public ICommand ResetBaseUri { get; }


    [Reactive] public string RegToken { get; private set; }
    [Reactive] public DateTime? RegDate { get; private set; }
    [Reactive] public AccessState AccessStatus { get; private set; }

    //public string BaseUri
    //{
    //    get => this.apiClient.BaseUri;
    //    set => this.apiClient.BaseUri = value;
    //}


    public override void OnAppearing()
    {
        base.OnAppearing();
        this.Refresh();
    }


    void Refresh() => this.RegToken = this.pushManager.RegistrationToken ?? "-";
}