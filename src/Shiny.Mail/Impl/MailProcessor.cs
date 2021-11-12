using System.Net.Mail;


namespace Shiny.Mail.Impl
{
    public class MailProcessor : IMailProcessor
    {
        readonly IMailSender mailSender;
        readonly IMailTemplateParser mailTemplateParser;
        readonly ITemplateLoader templateLoader;
        readonly ITemplateParser templateParser;


        public MailProcessor(IMailSender mailSender, 
                             IMailTemplateParser mailTemplateParser, 
                             ITemplateLoader templateLoader,
                             ITemplateParser templateParser)
        {
            this.mailSender = mailSender;
            this.mailTemplateParser = mailTemplateParser;
            this.templateLoader = templateLoader;
            this.templateParser = templateParser;
        }


        public async Task<MailMessage> Send(string templateName, object args, Action<MailMessage>? beforeSend = null)
        {
            var content = await this.templateLoader.Load(templateName).ConfigureAwait(false);
            content = await this.templateParser.Parse(content, args).ConfigureAwait(false);
            var mail = await this.mailTemplateParser.Parse(content).ConfigureAwait(false);
            beforeSend?.Invoke(mail);

            await this.mailSender.Send(mail).ConfigureAwait(false);
            return mail;
        }
    }
}
