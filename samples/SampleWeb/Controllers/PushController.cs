using Microsoft.AspNetCore.Mvc;
using SampleWeb.Contracts;
using Shiny.Api.Push;


namespace SampleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PushController : ControllerBase
    {
        readonly IPushManager pushManager;


        public PushController(IPushManager pushManager)
        {
            this.pushManager = pushManager;
        }


        [HttpPost("send")]
        public async Task<ActionResult> Send([FromBody] Contracts.Notification notification)
        {
            await this.pushManager.Send(
                new Shiny.Api.Push.Notification
                {
                    Title = notification.Title,
                    Message = notification.Message,
                    Data = notification.Data
                },
                new PushFilter
                {
                    DeviceToken = notification.DeviceToken,
                    UserId = notification.UserId,
                    Platform = notification.Platform,
                    Tags = notification.Tags
                }
            );
            return this.Ok();
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] Registration register)
        {
            await this.pushManager.Register(new PushRegistration
            {
                DeviceToken = register.DeviceToken,
                UserId = register.UserId,
                Platform = register.Platform,
                Tags = register.Tags
            });
            return this.Ok();
        }


        [HttpPost("unregister")]
        public async Task<ActionResult> UnRegister([FromBody] Registration register)
        {
            await this.pushManager.UnRegister(new PushFilter
            {
                DeviceToken = register.DeviceToken,                
                UserId = register.UserId,
                Platform = register.Platform,
                Tags = register.Tags
            });
            return this.Ok();
        }
    }
}
