using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public class WindowsNotificationPushProvider : IPushProvider
    {
        public PushPlatform Platform { get; } = PushPlatform.Microsoft;


        public Task Send(string deviceToken, Notification notification)
        {
            throw new System.NotImplementedException();
        }
    }
}
