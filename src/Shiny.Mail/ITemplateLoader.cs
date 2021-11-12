namespace Shiny.Mail
{
    public interface ITemplateLoader
    {
        Task<string> Load(string templateName);
    }
}
