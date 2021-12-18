using System.Net.Mail;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    public interface IMailSender
    {
        Task Send(MailMessage mail);
    }
}