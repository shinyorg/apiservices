using Microsoft.Extensions.DependencyInjection;


namespace Shiny.Extensions.Mail
{
    public static class MailConfiguratorExtensions
    {
        public static MailConfigurator UseTemplateConverter<TImpl>(this MailConfigurator config) where TImpl : class, IMailTemplateConverter
        {
            config.Services.AddScoped<IMailTemplateConverter, TImpl>();
            return config;
        }


        public static MailConfigurator UseSender<TSender>(this MailConfigurator config) where TSender : class, IMailSender
        {
            config.Services.AddScoped<IMailSender, TSender>();
            return config;
        }


        public static MailConfigurator UseEngine<TImpl>(this MailConfigurator config) where TImpl : class, IMailEngine
        {
            config.Services.AddScoped<IMailEngine, TImpl>();
            return config;
        }
    }
}
