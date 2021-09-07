using System.Threading.Tasks;


namespace Shiny.Api.Push
{
    public interface IPushProvider
    {
        Task Send(Notification notification);
    }
}
