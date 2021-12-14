using Microsoft.Extensions.DependencyInjection;

using Shiny.Mail.Impl;

namespace Shiny.Mail
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMailProcessor(this IServiceCollection services, Action<MailConfigurator> configAction)
        {
            var config = new MailConfigurator(services);
            configAction.Invoke(config);

            if (!services.Any(x => x.ServiceType == typeof(ITemplateParser)))
                services.AddSingleton<ITemplateParser, RazorTemplateParser>();

            if (!services.Any(x => x.ServiceType == typeof(IMailTemplateParser)))
                services.AddTransient<IMailTemplateParser, FrontMatterMailTemplateParser>();

            if (!services.Any(x => x.ServiceType == typeof(IMailProcessor)))
                services.AddTransient<IMailProcessor, MailProcessor>();
        }
    }
}
