using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Mail.Impl;


namespace Shiny.Mail
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMailProcessor(this IServiceCollection services, Action<MailConfigurator> configAction)
        {
            var config = new MailConfigurator(services);
            configAction.Invoke(config);

            services.TryAddSingleton<ITemplateParser, RazorTemplateParser>();
            services.TryAddSingleton<IMailTemplateParser, FrontMatterMailTemplateParser>();
            services.TryAddTransient<IMailProcessor, MailProcessor>();
        }
    }
}
