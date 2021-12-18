using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Providers
{
    public interface IPushProvider<TNative> where TNative : class
    {
        TNative CreateNativeNotification(Notification notification);

        /// <summary>
        /// Sends to the notification to the native platform
        /// </summary>
        /// <param name="token"></param>
        /// <param name="notification">The notification configuration</param>
        /// <param name="native">The full native configuration to send</param>
        /// <param name="cancelToken"></param>
        /// <returns>True if send is detected, false if the provider reports no result - false does not indicate an error</returns>
        Task<bool> Send(string token, Notification notification, TNative native, CancellationToken cancelToken);
    }
}
