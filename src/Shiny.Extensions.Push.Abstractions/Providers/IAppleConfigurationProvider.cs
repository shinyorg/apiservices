using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Providers
{
    public interface IAppleConfigurationProvider
    {
        Task<AppleConfiguration> GetConfiguration(Notification notification);
    }
}
