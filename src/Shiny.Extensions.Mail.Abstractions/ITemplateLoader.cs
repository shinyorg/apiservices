using System.Threading.Tasks;


namespace Shiny.Extensions.Mail
{
    public interface ITemplateLoader
    {
        Task<string> Load(string templateName);
    }
}
