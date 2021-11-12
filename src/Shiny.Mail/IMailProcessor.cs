using System.Net.Mail;

namespace Shiny.Mail
{
    public interface IMailProcessor
    {
        Task<MailMessage> Send(string templateName, object args, Action<MailMessage>? beforeSend = null);
    }
}
