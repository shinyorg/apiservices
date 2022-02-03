using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Mail.Impl;
using System;
using System.Linq;


namespace Shiny.Extensions.Mail
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMail(this IServiceCollection services, Action<MailConfigurator> configAction)
        {
            var config = new MailConfigurator(services);
            configAction.Invoke(config);

            services.TryAddScoped<ITemplateParser, RazorTemplateParser>();
            services.TryAddScoped<IMailTemplateConverter, FrontMatterMailTemplateConverter>();
            services.TryAddScoped<IMailEngine, MailEngine>();

            if (!services.Any(x => x.ServiceType == typeof(ITemplateLoader)))
                throw new InvalidOperationException("No ITemplateLoader service has been registered");
        }


        public static MailConfigurator UseSmtpSender(this MailConfigurator cfg, SmtpConfig config)
        {
            cfg.Services.AddScoped<IMailSender>(_ => new SmtpMailSender(config));
            return cfg;
        }


        public static MailConfigurator UseSendGridSender(this MailConfigurator cfg, string apiKey)
        {
            cfg.Services.AddScoped<IMailSender>(_ => new SendGridMailSender(apiKey));
            return cfg;
        }


        public static MailConfigurator UseFileTemplateLoader(this MailConfigurator cfg, string path, string ext = "mailtemplate")
        {
            cfg.Services.AddScoped<ITemplateLoader>(_ => new FileTemplateLoader(path, ext));
            return cfg;
        }


        public static MailConfigurator UseSqlServerTemplateLoader(this MailConfigurator cfg, string connectionString)
        {
            cfg.Services.AddScoped<ITemplateLoader>(_ => new SqlServerTemplateLoader(connectionString));
            return cfg;
        }
    }
}
