using System.Net.Mail;

namespace Shiny.Mail.Impl
{
    public class FrontMatterMailTemplateParser : IMailTemplateParser
    {
        public async Task<MailMessage> Parse(string content)
        {
            var mail = new MailMessage();
            using (var reader = new StringReader(content))
            {
                bool endOfFrontMatter = false;
                var line = reader.ReadLine();

                while (line != null && !endOfFrontMatter)
                {

                }
                mail.Body = await reader.ReadToEndAsync().ConfigureAwait(false);
                //mail.AlternateViews.Add(new AlternateView())
            }
            return mail;
        }
    }
}
