using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


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


        public Task<string> Load(string templateName, CultureInfo? culture = null, CancellationToken cancellationToken = default)
        {
            var fullPath = this.FindTemplatePath(templateName, culture);
            return File.ReadAllTextAsync(fullPath, cancellationToken);
        }


        protected virtual string FindTemplatePath(string templateName, CultureInfo? culture)
        {
            if (culture == null)
                return this.GetDefaultTemplatePath(templateName);

            string path;
            if (this.TryGetCultureTemplatePath(templateName, culture.Name, out path))
                return path;

            if (this.TryGetCultureTemplatePath(templateName, culture.TwoLetterISOLanguageName, out path))
                return path;

            return this.GetDefaultTemplatePath(templateName);
        }


        protected virtual string GetDefaultTemplatePath(string templateName)
        {
            var tn = $"{templateName}.{this.ext}";
            var fullPath = Path.Combine(this.path, tn);
            if (!File.Exists(fullPath))
                throw new ArgumentException($"Template '{fullPath}' not found");

            return fullPath;
        }


        protected virtual bool TryGetCultureTemplatePath(string templateName, string cultureCode, out string fullTemplatePath)
        {
            fullTemplatePath = null;

            var tn = $"{templateName}-{cultureCode}.{this.ext}";
            var fullPath = Path.Combine(this.path, tn);
            if (File.Exists(fullPath))
            {
                fullTemplatePath = fullPath;
                return true;
            }
            return false;
        }
    }
}
