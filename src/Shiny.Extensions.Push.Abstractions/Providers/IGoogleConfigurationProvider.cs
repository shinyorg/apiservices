using System.Threading.Tasks;

namespace Shiny.Extensions.Push.Providers
{
    public interface IGoogleConfigurationProvider
    {
        Task<GoogleConfiguration> GetConfiguration(Notification notification);
    }
}
