
namespace Shiny.Mail.Impl
{
    public class FileTemplateLoader : ITemplateLoader
    {
        readonly string path;


        public FileTemplateLoader(string path)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException($"The path {path} does not exist");

            this.path = path;
        }


        public Task<string> Load(string templateName)
        {
            var path = Path.Combine(this.path, templateName);
            return File.ReadAllTextAsync(path);
        }
    }
}
