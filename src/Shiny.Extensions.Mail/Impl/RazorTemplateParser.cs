using RazorEngine.Configuration;
using RazorEngine.Templating;
using Shiny.Extensions.Mail.Impl.Helpers;
using System;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class RazorTemplateParser : ITemplateParser
    {
        readonly IRazorEngineService service;


        public RazorTemplateParser(TemplateServiceConfiguration? config = null)
        {
            config ??= new TemplateServiceConfiguration();
            config.Namespaces.Add(typeof(RazorTemplateHelper).Namespace);

            this.service = RazorEngineService.Create(config);
        }


        public Task<string> Parse(string content, object args)
        {
            var newContent = this.service.RunCompile(content, Guid.NewGuid().ToString(), null, args);
            return Task.FromResult(newContent);
        }
    }
}