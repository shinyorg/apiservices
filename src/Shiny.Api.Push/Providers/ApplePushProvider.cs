using System.Threading.Tasks;


namespace Shiny.Api.Push.Providers
{
    public class ApplePushProvider : IPushProvider
    {
        public Task Send(Notification notification)
        {
            throw new System.NotImplementedException();
        }
    }
}
