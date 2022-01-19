using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Mail.DotLiquid;


namespace Shiny.Extensions.Mail
{
    public static class ServiceCollectionExtensions
    {
        public static MailConfigurator UseDotLiquidTemplateParser(this MailConfigurator cfg)
        {
            cfg.Services.AddSingleton<ITemplateParser, DotLiquidTemplateParser>();
            return cfg;
        }
    }
}
