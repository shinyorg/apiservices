using Shiny;
using Shiny.Push;
using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Prism.Navigation;
using SampleWeb.Contracts;


namespace SampleMobile.Push
{
    public class CreatePushViewModel : ViewModel
    {
        public CreatePushViewModel(ISampleApi api,
                                   INavigationService navigator,
                                   IPushManager pushManager,
                                   IPlatform platform,
                                   IDialogs dialogs)
        {
            this.ShowRegistrations = navigator.NavigateCommand(nameof(RegistrationListPage), p =>
            {
                var filter = new Registration
                {
                    DeviceToken = this.DeviceToken!,
                    UserId = this.UserId!,
                    Tags = this.Tags?.Split(','),
                    UseAndroid = this.SendToAndroid,
                    UseApple = this.SendToIos
                };
                p.Add("Filter", filter);
            });

            this.SetDeviceTokenToMe = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await pushManager.RequestAccess();
                if (result.Status == AccessState.Available)
                    this.DeviceToken = result.RegistrationToken!;
                else
                    await this.Dialogs.Alert("Invalid Push Permission - " + result.Status);
            });

            this.Register = ReactiveCommand.CreateFromTask(() => dialogs.LoadingTask(async () =>
            {
                var result = await pushManager.RequestAccess();
                if (result.Status != AccessState.Available)
                {
                    await this.Dialogs.Alert("Invalid Push Permission - " + result.Status);
                }
                else
                {
                    await api.Register(new SampleWeb.Contracts.Registration
                    {
                        DeviceToken = result.RegistrationToken!,
                        UserId = this.UserId,
                        UseAndroid = platform.IsAndroid(),
                        UseApple = platform.IsIos(),
                        Tags = this.Tags?.Split(',')
                    });
                }
            }));

            this.UnRegister = ReactiveCommand.CreateFromTask(async () =>
            {
                await pushManager.UnRegister();
                await dialogs.Snackbar("UnRegistered successfully");
            });

            this.Send = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var notification = new SampleWeb.Contracts.Notification
                    {
                        Title = this.NotificationTitle,
                        Message = this.NotificationMessage,

                        DeviceToken = this.DeviceToken!,
                        UserId = this.UserId!,
                        Tags = this.Tags!.Split(','),
                        UseAndroid = this.SendToAndroid,
                        UseApple = this.SendToIos
                    };
                    if (!this.DataKey.IsEmpty() && !this.DataValue.IsEmpty())
                        notification.Data = new Dictionary<string, string> {{ this.DataKey!, this.DataValue! }};

                    await this.Dialogs.LoadingTask(() => api.Send(notification));
                    await dialogs.Snackbar("Notification Sent");
                },
                this.WhenAny(
                    x => x.NotificationMessage,
                    x => x.SendToAndroid,
                    x => x.SendToIos,
                    (msg, android, ios) =>
                        !msg.GetValue().IsEmpty() &&
                        (android.GetValue() || ios.GetValue())
                )
            );
        }


        public ICommand Send { get; }
        public ICommand Register { get; }
        public ICommand UnRegister { get; }
        public ICommand SetDeviceTokenToMe { get; }
        public ICommand ShowRegistrations { get; }

        [Reactive] public string NotificationTitle { get; set; }
        [Reactive] public string NotificationMessage { get; set; }
        [Reactive] public string DataKey { get; set; }
        [Reactive] public string DataValue { get; set; }

        [Reactive] public string DeviceToken { get; set; }
        [Reactive] public string UserId { get; set; }
        [Reactive] public string Tags { get; set; }
        [Reactive] public bool SendToIos { get; set; } = true;
        [Reactive] public bool SendToAndroid { get; set; } = true;
    }
}
