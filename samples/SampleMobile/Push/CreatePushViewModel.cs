using Shiny;
using Shiny.Push;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace SampleMobile.Push
{
    public class CreatePushViewModel : ViewModel
    {
        public CreatePushViewModel(ISampleApi api, IPushManager pushManager, IDialogs dialogs)
        {
            this.SetDeviceTokenToMe = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await pushManager.RequestAccess();
                if (result.Status == AccessState.Available)
                { 
                    this.DeviceToken = result.RegistrationToken!;
                }
                else
                {
                    await this.Dialogs.Alert("Invalid Push Permission - " + result.Status);
                }
            });

            this.Send = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var result = await pushManager.RequestAccess();
                    if (result.Status != AccessState.Available)
                    {

                        await this.Dialogs.LoadingTask(() => api.Send(new SampleWeb.Contracts.Notification
                        {
                            Title = this.Title,
                            Message = this.Message,

                            DeviceToken = this.DeviceToken,
                            UserId = this.UserId,
                            Tags = this.Tags.Split(',')
                        }));
                        await dialogs.Snackbar("Notification Sent");
                    }
                }, 
                this.WhenAny(
                    x => x.Message,
                    x => x.SendToAndroid,
                    x => x.SendToIos,
                    (msg, android, ios) => 
                        !msg.GetValue().IsEmpty() &&
                        (android.GetValue() || ios.GetValue())
                )
            );
        }


        public ICommand Send { get; set; }
        public ICommand SetDeviceTokenToMe { get; set; }

        [Reactive] public string Title { get; set; }
        [Reactive] public string Message { get; set; }
        // TODO: push data

        [Reactive] public string DeviceToken { get; set; }
        [Reactive] public string UserId { get; set; }
        [Reactive] public string Tags { get; set; }
        [Reactive] public bool SendToIos { get; set; } = true;
        [Reactive] public bool SendToAndroid { get; set; } = true;
    }
}
