using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    public interface ITemplateParser
    {
        Task<string> Parse(string content, object args);
    }
}
