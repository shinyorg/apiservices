using Shiny.Extensions.Push.Providers;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class ConfiguredGoogleConfigurationProvider : IGoogleConfigurationProvider
    {
        readonly GoogleConfiguration configuration;
        public ConfiguredGoogleConfigurationProvider(GoogleConfiguration configuration)
            => this.configuration = configuration;

        public Task<GoogleConfiguration> GetConfiguration(Notification notification)
            => Task.FromResult(this.configuration);
    }
}
