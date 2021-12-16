
namespace Shiny.Extensions.Mail.Impl
{
    public class FileTemplateLoader : ITemplateLoader
    {
        readonly string ext;
        readonly string path;


        public FileTemplateLoader(string path, string ext = "mailtemplate")
        {
            if (!Directory.Exists(path))
                throw new ArgumentException($"The path {path} does not exist");

            this.path = path;
            this.ext = ext;
        }


        public Task<string> Load(string templateName)
        {
            var tn = $"{templateName}.{this.ext}";
            var fullPath = Path.Combine(this.path, tn);
            return File.ReadAllTextAsync(fullPath);
        }
    }
}
