using System.Threading.Tasks;

namespace Shiny.Api.Push.Management.Providers
{
    public interface IGooglePushProvider
    {
        Task Send(GoogleNotification notification);
    }
}
