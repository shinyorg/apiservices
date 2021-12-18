using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    /// <summary>
    /// Responsible for parsing the raw string content of the file (pre-processing)
    /// </summary>
    public interface ITemplateParser
    {
        Task<string> Parse(string content, object args, CultureInfo? culture, CancellationToken cancellationToken = default);
    }
}
