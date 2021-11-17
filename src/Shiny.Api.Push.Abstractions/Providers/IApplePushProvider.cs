using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public interface IApplePushProvider
    {
        Task Send(AppleNotification notification);
    }
}
