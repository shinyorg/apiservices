using System.Threading.Tasks;


namespace Shiny.Api.Push
{
    public interface IPushProvider
    {
        PushPlatform Platform { get; }
        Task Send(string deviceToken, Notification notification);
    }
}
