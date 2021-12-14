using RazorEngine.Configuration;
using RazorEngine.Templating;


namespace Shiny.Mail.Impl
{
    public class RazorTemplateParser : ITemplateParser
    {
        readonly IRazorEngineService service;


        public RazorTemplateParser(TemplateServiceConfiguration? config = null)
        {
            config ??= new TemplateServiceConfiguration();
            //config.Namespaces.Add("");
            this.service = RazorEngineService.Create(config);
        }


        public Task<string> Parse(string content, object args)
        {
            var newContent = this.service.RunCompile(content, Guid.NewGuid().ToString(), null, args);
            return Task.FromResult(newContent);
        }
    }
}