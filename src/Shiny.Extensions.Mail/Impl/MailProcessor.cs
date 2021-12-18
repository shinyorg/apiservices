using System;
using System.Globalization;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class MailProcessor : IMailProcessor
    {
        readonly IMailTemplateConverter mailTemplateConverter;
        readonly ITemplateLoader templateLoader;
        readonly ITemplateParser templateParser;


        public MailProcessor(IMailSender mailSender,
                             IMailTemplateConverter mailTemplateConverter,
                             ITemplateLoader templateLoader,
                             ITemplateParser templateParser)
        {
            this.Sender = mailSender;
            this.mailTemplateConverter = mailTemplateConverter;
            this.templateLoader = templateLoader;
            this.templateParser = templateParser;
        }


        public IMailSender Sender { get; }


        public async Task<MailMessage> Parse(string templateName, object args, CultureInfo? culture = null, CancellationToken cancellationToken = default)
        {
            var content = await this.templateLoader.Load(templateName, culture, cancellationToken).ConfigureAwait(false);
            content = await this.templateParser.Parse(content, args, culture, cancellationToken).ConfigureAwait(false);
            var mail = await this.mailTemplateConverter.Convert(content, culture, cancellationToken).ConfigureAwait(false);
            return mail;
        }


        public async Task<MailMessage> Send(string templateName, object args, CultureInfo? culture = null, Action<MailMessage>? beforeSend = null, CancellationToken cancellationToken = default)
        {
            var mail = await this.Parse(templateName, args, culture, cancellationToken).ConfigureAwait(false);
            beforeSend?.Invoke(mail);

            // could validate email here as the template may not have had address in it
            await this.Sender.Send(mail, cancellationToken).ConfigureAwait(false);
            return mail;
        }
    }
}
