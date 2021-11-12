using System.Net.Mail;


namespace Shiny.Mail
{
    public interface IMailSender
    {
        Task Send(MailMessage mail);
    }
}