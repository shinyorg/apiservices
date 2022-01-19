using Microsoft.AspNetCore.Mvc;
using SampleWeb.Contracts;
using Shiny.Extensions.Mail;
using System.Net.Mail;


namespace SampleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        readonly IMailEngine mailProcessor;
        public MailController(IMailEngine mailProcessor)
        {
            this.mailProcessor = mailProcessor;
        }


        [HttpPost("parse/{templateName?}")]
        public async Task<ActionResult<MailMessage>> Parse([FromBody] SendMail args, string templateName = "test")
        {
            var mail = await this.mailProcessor.Parse(templateName, args);
            return this.FromMail(mail);
        }


        [HttpPost("send/{templateName?}")]
        public async Task<ActionResult<MailMessage>> Send([FromBody] SendMail args, string templateName = "test")
        {
            var mail = await this.mailProcessor.Send(templateName, args);
            return this.FromMail(mail);
        }


        ActionResult FromMail(MailMessage msg)
        {
            var tmp = new MailTemplateResult
            {
                From = ToAddress(msg.From),
                To = ToAddresses(msg.To),
                Cc = ToAddresses(msg.CC),
                Bcc = ToAddresses(msg.Bcc),
                Subject = msg.Subject,
                Body = msg.Body,
            };
            return this.Ok(tmp);
        }


        static Address ToAddress(MailAddress add)
            => new Address { DisplayName = add.DisplayName, Value = add.Address };

        static Address[] ToAddresses(MailAddressCollection adds)
            => adds.Select(ToAddress).ToArray();
    }
}
