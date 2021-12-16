using System.Net.Mail;


namespace Shiny.Extensions.Mail
{
    public interface IMailTemplateParser
    {
        Task<MailMessage> Parse(string content);
    }
}
