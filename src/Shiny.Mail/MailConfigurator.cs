using Microsoft.Extensions.DependencyInjection;

namespace Shiny.Mail
{
    public class MailConfigurator
    {
        public MailConfigurator(IServiceCollection services) => this.Services = services;
        public IServiceCollection Services { get; }


        public MailConfigurator UseSmtpSender()
        {
            return this;
        }


        public MailConfigurator AddSender<TSender>() where TSender : IMailSender
        {
            return this;
        }
    }
}
