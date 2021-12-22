using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class ResxTemplateLoader : ITemplateLoader
    {
        readonly ResourceManager resourceManager;


        public ResxTemplateLoader(string baseName, Assembly assembly)
        {
            this.resourceManager = new ResourceManager(baseName, assembly);
        }


        public Task<string?> Load(string templateName, CultureInfo? culture, CancellationToken cancellationToken = default)
        {
            var content = this.resourceManager.GetString(templateName, culture);
            if (content == null)
                throw new ArgumentException($"Template {templateName} not found");

            return Task.FromResult<string?>(content);
        }
    }
}
