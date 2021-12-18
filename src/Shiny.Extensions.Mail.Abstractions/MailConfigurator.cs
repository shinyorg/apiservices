using Microsoft.Extensions.DependencyInjection;


namespace Shiny.Extensions.Mail
{ 
    public class MailConfigurator
    {
        public MailConfigurator(IServiceCollection services) => this.Services = services;
        public IServiceCollection Services { get; }


        public MailConfigurator UseMailTemplateParser<TImpl>() where TImpl : class, IMailTemplateParser
        {
            this.Services.AddSingleton<IMailTemplateParser, TImpl>();
            return this;
        }


        public MailConfigurator UseSender<TSender>() where TSender : class, IMailSender
        {
            this.Services.AddSingleton<IMailSender, TSender>();
            return this;
        }


        public MailConfigurator UseMailProcessor<TImpl>() where TImpl : class, IMailProcessor
        {
            this.Services.AddSingleton<IMailProcessor, TImpl>();
            return this;
        }
    }
}