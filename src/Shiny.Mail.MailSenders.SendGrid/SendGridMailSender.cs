using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;


namespace Shiny.Mail.MailSenders.SendGrid
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

            //msg.HtmlContent = mail.Body;
            //msg.PlainTextContent = mail.Body;

            foreach (var replyTo in mail.ReplyToList)
                msg.SetReplyTo(new EmailAddress(replyTo.Address, replyTo.DisplayName));

            foreach (var to in mail.To)
                msg.AddTo(to.Address, to.DisplayName);

            foreach (var cc in mail.CC)
                msg.AddCc(cc.Address, cc.DisplayName);

            foreach (var bcc in mail.Bcc)
                msg.AddBcc(bcc.Address, bcc.DisplayName);

            //foreach (var att in mail.Attachments)
            //    msg.AddAttachment(att.Name, att.ContentStream.ToString, att.ContentType, att.ContentDisposition, att.ContentId);

            
            var response = await client.SendEmailAsync(msg);
        }
    }
}