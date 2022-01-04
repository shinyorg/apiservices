using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Mail;
using Shiny.Extensions.Mail.Impl;
using System;


namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMailProcessor(this IServiceCollection services, Action<MailConfigurator> configAction)
        {
            var config = new MailConfigurator(services);
            configAction.Invoke(config);

            services.TryAddSingleton<ITemplateParser, RazorTemplateParser>();
            services.TryAddSingleton<IMailTemplateConverter, FrontMatterMailTemplateConverter>();
            services.TryAddSingleton<IMailProcessor, MailProcessor>();
        }


        public static MailConfigurator UseSmtpSender(this MailConfigurator cfg, SmtpConfig config)
        {
            cfg.Services.AddSingleton<IMailSender>(_ => new SmtpMailSender(config));
            return cfg;
        }


        public static MailConfigurator UseSendGridSender(this MailConfigurator cfg, string apiKey)
        {
            cfg.Services.AddSingleton<IMailSender>(_ => new SendGridMailSender(apiKey));
            return cfg;
        }


        public static MailConfigurator UseFileTemplateLoader(this MailConfigurator cfg, string path, string ext = "mailtemplate")
        {
            cfg.Services.AddSingleton<ITemplateLoader>(_ => new FileTemplateLoader(path, ext));
            return cfg;
        }


        public static MailConfigurator UseSqlServerTemplateLoader(this MailConfigurator cfg, string connectionString)
        {
            cfg.Services.AddSingleton<ITemplateLoader>(_ => new SqlServerTemplateLoader(connectionString));
            return cfg;
        }
    }
}
