using System.Threading.Tasks;

namespace Shiny.Api.Push.Management.Providers
{
    public interface IApplePushProvider
    {
        Task Send(AppleNotification notification);
    }
}
