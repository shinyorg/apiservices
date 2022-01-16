using Shiny.Extensions.Push.Providers;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class ConfiguredAppleConfigurationProvider : IAppleConfigurationProvider
    {
        readonly AppleConfiguration configuration;
        public ConfiguredAppleConfigurationProvider(AppleConfiguration configuration)
            => this.configuration = configuration;


        public Task<AppleConfiguration> GetConfiguration(Notification notification)
            => Task.FromResult(this.configuration);
    }
}
