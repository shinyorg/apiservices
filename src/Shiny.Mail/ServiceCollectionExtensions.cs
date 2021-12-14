using Microsoft.Extensions.DependencyInjection;


namespace Shiny.Mail
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMailProcessor(this IServiceCollection services, Action<MailConfigurator> configAction)
        {
            var config = new MailConfigurator(services);
            configAction.Invoke(config);
        }
    }
}
