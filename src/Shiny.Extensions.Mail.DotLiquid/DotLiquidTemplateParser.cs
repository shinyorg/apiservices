using DotLiquid;

using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.DotLiquid
{
    public class DotLiquidTemplateParser : ITemplateParser
    {
        //https://github.com/dotliquid/dotliquid/wiki/DotLiquid-for-Developers
        public Task<string> Parse(string content, object args, CultureInfo? culture, CancellationToken cancellationToken = default)
        {
            // TODO: helper functions
            var template = Template.Parse(content);
            var result = template.Render(Hash.FromAnonymousObject(args));
            return Task.FromResult(result);
        }
    }
}