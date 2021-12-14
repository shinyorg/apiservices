using Microsoft.AspNetCore.Mvc;
using Shiny.Mail;


namespace SampleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        readonly IMailProcessor mailProcessor;
        public MailController(IMailProcessor mailProcessor)
        {
            this.mailProcessor = mailProcessor;
        }
    }
}
