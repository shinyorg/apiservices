using System.Threading.Tasks;


namespace Shiny.Api.Push
{
    public interface IPushProvider
    {
        Task Send(string deviceToken, Notification notification);
    }
}
