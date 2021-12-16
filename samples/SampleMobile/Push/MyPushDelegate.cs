using System.Threading.Tasks;
using Shiny.Push;


namespace SampleMobile.Push
{
    public class MyPushDelegate : IPushDelegate
    {
        public Task OnEntry(PushNotificationResponse response) => Task.CompletedTask;
        public Task OnReceived(PushNotification notification) => Task.CompletedTask;
        public Task OnTokenRefreshed(string token) => Task.CompletedTask;
    }
}
