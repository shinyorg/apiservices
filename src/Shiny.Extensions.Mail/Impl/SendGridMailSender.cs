using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class SendGridMailSender : IMailSender
    {
        readonly string apiKey;
        public SendGridMailSender(string apiKey) => this.apiKey = apiKey;


        public async Task Send(MailMessage mail)
        {
            var client = new SendGridClient(this.apiKey);
            var msg = new SendGridMessage();

            msg.Subject = mail.Subject;
            msg.SetFrom(mail.From.Address, mail.From.DisplayName);

            if (mail.IsBodyHtml)
            {
                msg.HtmlContent = mail.Body;
            }
            else
            {
                msg.PlainTextContent = mail.Body;
            }

            foreach (var replyTo in mail.ReplyToList)
                msg.SetReplyTo(new EmailAddress(replyTo.Address, replyTo.DisplayName));

            foreach (var to in mail.To)
                msg.AddTo(to.Address, to.DisplayName);

            foreach (var cc in mail.CC)
                msg.AddCc(cc.Address, cc.DisplayName);

            foreach (var bcc in mail.Bcc)
                msg.AddBcc(bcc.Address, bcc.DisplayName);

            foreach (var att in mail.Attachments)
            {
                msg.AddAttachment(
                    att.Name,
                    ToAttachment(att.ContentStream),
                    att.ContentType?.MediaType,
                    att.ContentDisposition?.DispositionType,
                    att.ContentId
                );
            }

            var response = await client
                .SendEmailAsync(msg)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new ArgumentException("Failed Response from SendGrid - " + response.StatusCode);
        }


        static string ToAttachment(Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            var bytes = ms.ToArray();
            return Convert.ToBase64String(bytes);
        }
    }
}