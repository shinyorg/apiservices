using Microsoft.AspNetCore.Mvc;
using SampleWeb.Contracts;
using Shiny.Extensions.Push;


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


        [HttpPost("registrations")]
        public async Task<ActionResult<IEnumerable<PushRegistration>>> Registrations([FromBody] PushFilter filter)
        {
            var result = await this.pushManager.GetRegistrations(filter);
            return this.Ok(result);
        }


        [HttpPost("send")]
        public async Task<ActionResult> Send([FromBody] Contracts.Notification notification)
        {
            await this.pushManager.Send(
                new Shiny.Extensions.Push.Notification
                {
                    Title = notification.Title,
                    Message = notification.Message,
                    Data = notification.Data
                },
                ToFilter(notification)
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
                Platform = GetPlatform(register),
                Tags = register.Tags
            });
            return this.Ok();
        }


        [HttpPost("unregister")]
        public async Task<ActionResult> UnRegister([FromBody] Registration register)
        {
            await this.pushManager.UnRegister(ToFilter(register));
            return this.Ok();
        }


        static PushFilter ToFilter(Registration reg) => new PushFilter
        {
            DeviceToken = reg.DeviceToken,
            UserId = reg.UserId,
            Tags = reg.Tags,
            Platform = GetPlatform(reg)
        };


        static PushPlatforms GetPlatform(Registration reg)
        {
            if (reg.UseApple && reg.UseAndroid)
                return PushPlatforms.All;

            if (reg.UseAndroid)
                return PushPlatforms.Google;

            if (reg.UseApple)
                return PushPlatforms.Apple;

            throw new ArgumentException("Not platform set");
        }
    }
}
