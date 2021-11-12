using System.Net.Mail;


namespace Shiny.Mail
{
    public interface IMailTemplateParser
    {
        Task<MailMessage> Parse(string content);
    }
}
