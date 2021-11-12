namespace Shiny.Mail.Impl
{
    public class DictionaryReplaceTemplateParser : ITemplateParser
    {
        public Task<string> Parse(string content, object args)
        {
            if (args is not IDictionary<string, object> dict)
                throw new ArgumentException("You can only pass IDictionary<string, object> to this template parser");

            foreach (var key in dict.Keys)
                content = content.Replace("{" + key + "}", dict[key].ToString());

            return Task.FromResult(content);
        }
    }
}
