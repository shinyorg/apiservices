using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public class WindowsNotificationPushProvider : IPushProvider
    {
        public Task Send(string deviceToken, Notification notification)
        {
            throw new System.NotImplementedException();
        }
    }
}
