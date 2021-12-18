using System;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    public interface IMailProcessor
    {
        IMailSender Sender { get; }

        Task<MailMessage> Parse(string templateName, object args);
        Task<MailMessage> Send(string templateName, object args, Action<MailMessage>? beforeSend = null);
    }
}
