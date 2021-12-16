using System.Net.Mail;

namespace Shiny.Extensions.Mail
{
    public interface IMailProcessor
    {
        IMailSender Sender { get; }

        Task<MailMessage> Parse(string templateName, object args);
        Task<MailMessage> Send(string templateName, object args, Action<MailMessage>? beforeSend = null);
    }
}
