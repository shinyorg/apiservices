using System;
using System.Threading.Tasks;
using Shiny.Push;


namespace SampleMobile
{
    public class MyPushDelegate : IPushDelegate
    {
        public Task OnEntry(PushNotificationResponse response)
        {
            throw new NotImplementedException();
        }

        public Task OnReceived(PushNotification notification)
        {
            throw new NotImplementedException();
        }

        public Task OnTokenRefreshed(string token)
        {
            throw new NotImplementedException();
        }
    }
}
