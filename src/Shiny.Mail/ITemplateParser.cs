using System.Threading.Tasks;


namespace Shiny.Mail
{
    public interface ITemplateParser
    {
        Task<string> Parse(string content, object args);
    }
}
