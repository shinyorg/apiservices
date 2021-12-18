using System.Globalization;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    public interface IMailTemplateConverter
    {
        /// <summary>
        /// Responsible for converting the string content file into a MailTemplate
        /// </summary>
        /// <param name="content"></param>
        /// <param name="culture"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MailMessage> Convert(string content, CultureInfo? culture = null, CancellationToken cancellationToken = default);
    }
}
