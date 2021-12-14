namespace Shiny.Mail;

using Microsoft.Extensions.DependencyInjection;
using Shiny.Mail.Impl;

public class MailConfigurator
{
    public MailConfigurator(IServiceCollection services) => this.Services = services;
    public IServiceCollection Services { get; }

    public MailConfigurator UseMailProcessor<TImpl>() where TImpl : class, IMailProcessor
    {
        this.Services.AddTransient<IMailProcessor, MailProcessor>();
        return this;
    }


    public MailConfigurator UseMailTemplateParser<TImpl>() where TImpl : class, IMailTemplateParser
    {
        this.Services.AddSingleton<IMailTemplateParser, TImpl>();
        return this;
    }

    public MailConfigurator UseSmtpSender(SmtpConfig config)
    {
        this.Services.AddSingleton<IMailSender>(_ => new SmtpMailSender(config));
        return this;
    }


    public MailConfigurator UseSender<TSender>() where TSender : class, IMailSender
    {
        this.Services.AddSingleton<IMailSender, TSender>();
        return this;
    }


    public MailConfigurator UseSendGridSender(string apiKey)
    {
        this.Services.AddSingleton<IMailSender>(_ => new SendGridMailSender(apiKey));
        return this;
    }


    public MailConfigurator UseShinyStoreTemplateLoader()
    {
        this.Services.AddSingleton<ITemplateLoader, ShinyStorageTemplateLoader>();
        return this;
    }


    public MailConfigurator UseFileTemplateLoader(string path)
    {
        this.Services.AddSingleton<ITemplateLoader>(_ => new FileTemplateLoader(path));
        return this;
    }
}