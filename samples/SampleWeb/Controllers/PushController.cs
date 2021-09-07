using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace SampleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PushController : ControllerBase
    {
        readonly ILogger logger;


        public PushController(ILogger<PushController> logger)
        {
            this.logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Send()
        {
            return this.Ok();
        }
    }
}
