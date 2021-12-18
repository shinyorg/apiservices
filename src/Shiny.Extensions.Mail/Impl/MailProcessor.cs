using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class MailProcessor : IMailProcessor
    {
        readonly IMailTemplateParser mailTemplateParser;
        readonly ITemplateLoader templateLoader;
        readonly ITemplateParser templateParser;


        public MailProcessor(IMailSender mailSender,
                             IMailTemplateParser mailTemplateParser,
                             ITemplateLoader templateLoader,
                             ITemplateParser templateParser)
        {
            this.Sender = mailSender;
            this.mailTemplateParser = mailTemplateParser;
            this.templateLoader = templateLoader;
            this.templateParser = templateParser;
        }


        public IMailSender Sender { get; }


        public async Task<MailMessage> Parse(string templateName, object args, CancellationToken cancellationToken = default)
        {
            var content = await this.templateLoader.Load(templateName, cancellationToken).ConfigureAwait(false);
            content = await this.templateParser.Parse(content, args, cancellationToken).ConfigureAwait(false);
            var mail = await this.mailTemplateParser.Parse(content, cancellationToken).ConfigureAwait(false);
            return mail;
        }


        public async Task<MailMessage> Send(string templateName, object args, Action<MailMessage>? beforeSend = null, CancellationToken cancellationToken = default)
        {
            var mail = await this.Parse(templateName, args, cancellationToken).ConfigureAwait(false);
            beforeSend?.Invoke(mail);

            // could validate email here as the template may not have had address in it
            await this.Sender.Send(mail, cancellationToken).ConfigureAwait(false);
            return mail;
        }
    }
}
