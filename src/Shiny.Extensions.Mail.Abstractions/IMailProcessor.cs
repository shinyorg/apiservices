using System;
using System.Globalization;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    public interface IMailProcessor
    {
        IMailSender Sender { get; }

        Task<MailMessage> Parse(
            string templateName,
            object args,
            CultureInfo? culture = null,
            CancellationToken cancellationToken = default
        );

        Task<MailMessage> Send(
            string templateName,
            object args,
            CultureInfo? culture = null,
            Action<MailMessage>? beforeSend = null,
            CancellationToken cancellationToken = default
        );
    }
}
