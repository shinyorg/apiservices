using System.Net.Mail;

namespace Shiny.Mail.Impl
{
    // TODO: consider YAML parser in the future
    public class FrontMatterMailTemplateParser : IMailTemplateParser
    {
        public async Task<MailMessage> Parse(string content)
        {
            var mail = new MailMessage
            {
                IsBodyHtml = true // default this, set html: false if it is text only
            };
            //mail.Sender
            using (var reader = new StringReader(content))
            {
                var lineNum = 1;
                bool endOfFrontMatter = false;
                var line = reader.ReadLine();

                while (line != null && !endOfFrontMatter)
                {
                    if (line.StartsWith("---"))
                    {
                        endOfFrontMatter = true;
                    }
                    else
                    {
                        var values = line.Split(":");

                        SetOnMail(lineNum, values, mail);
                        line = reader.ReadLine();
                        lineNum++;
                    }
                }
                mail.Body = await reader
                    .ReadToEndAsync()
                    .ConfigureAwait(false);
            }
            return mail;
        }


        static void SetOnMail(int lineNum, string[] values, MailMessage mail)
        {
            if (values.Length != 2)
            {
                // ignore - template parser may have left it empty here - should log
                return;
            }

            var variable = values[0].ToLower().Trim();
            var value = values[1].Trim();
            if (String.IsNullOrWhiteSpace(variable) || String.IsNullOrWhiteSpace(value))
            {
                // also ignore
                return;
            }

            switch (variable)
            {
                case "subject":
                    mail.Subject = value;
                    break;

                case "html":
                    mail.IsBodyHtml = value.Equals("true", StringComparison.InvariantCultureIgnoreCase) || value.Equals("1");
                    break;

                case "from":
                    mail.From = ToAddress(value);
                    break;

                case "to":
                    AddToMailCollection(mail.To, value);
                    break;

                case "cc":
                    AddToMailCollection(mail.CC, value);
                    break;

                case "bcc":
                    AddToMailCollection(mail.Bcc, value);
                    break;

                case "replyto":
                    AddToMailCollection(mail.ReplyToList, value);
                    break;

                default:
                    throw new ArgumentException($"Invalid variable '{variable}' on line {lineNum}");
            }
        }


        static void AddToMailCollection(MailAddressCollection col, string value)
        {
            var addresses = value.Split(";").Select(x => x.Trim()).ToArray();
            foreach (var address in addresses)
            {
                var final = ToAddress(address);
                col.Add(final);
            }
        }


        static MailAddress ToAddress(string value)
        {
            var lt = value.LastIndexOf("<");
            var gt = value.LastIndexOf(">");
            var address = value;
            string? displayName = null;

            if (lt != -1 && gt != -1)
            {
                address = value.Substring(lt + 1).TrimEnd('>').Trim();
                displayName = value.Substring(0, lt - 1).Trim();
            }
            return displayName == null
                ? new MailAddress(value)
                : new MailAddress(address, displayName);
        }
    }
}
