using System.Threading.Tasks;

namespace Shiny.Api.Push
{
    public class PushNotificationManager : IPushNotificationManager
    {
        readonly IPushProvider[] providers;


        public PushNotificationManager(IPushProvider[] providers)
        {
            this.providers = providers;
        }


        public Task Send(Notification notification)
        {
            throw new System.NotImplementedException();
        }
    }
}
