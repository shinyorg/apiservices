using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public interface IGooglePushProvider
    {
        Task Send(string deviceToken, GoogleNotification notification);
    }
}
