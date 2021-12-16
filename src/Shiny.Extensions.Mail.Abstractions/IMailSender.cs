using System.Net.Mail;


namespace Shiny.Extensions.Mail
{
    public interface IMailSender
    {
        Task Send(MailMessage mail);
    }
}