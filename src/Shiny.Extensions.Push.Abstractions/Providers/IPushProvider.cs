using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Providers
{
    public interface IPushProvider<TNative> where TNative : class
    {
        TNative CreateNativeNotification(Notification notification);
        Task Send(string token, Notification notification, TNative native, CancellationToken cancelToken);
    }
}
