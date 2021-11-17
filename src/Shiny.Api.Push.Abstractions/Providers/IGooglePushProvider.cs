using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public interface IGooglePushProvider
    {
        Task Send(GoogleNotification notification);
    }
}
