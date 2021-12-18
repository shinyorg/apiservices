using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    public interface IMailTemplateParser
    {
        Task<MailMessage> Parse(string content, CancellationToken cancellationToken = default);
    }
}
