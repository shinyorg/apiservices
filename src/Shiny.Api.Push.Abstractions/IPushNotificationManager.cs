using System.Threading.Tasks;

namespace Shiny.Api.Push
{
    public interface IPushNotificationManager
    {
        Task Send(Notification notification);
    }
}
