using Microsoft.Extensions.DependencyInjection;


namespace Shiny.Extensions.Mail
{
    public class MailConfigurator
    {
        public MailConfigurator(IServiceCollection services) => this.Services = services;
        public IServiceCollection Services { get; }
    }
}