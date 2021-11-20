using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public interface IPushProvider<TNative> where TNative : class
    {
        TNative CreateNativeNotification(Notification notification);
        Task Send(string token, TNative native);
    }
}
