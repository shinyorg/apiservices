using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace SampleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
        readonly ILogger logger;


        public TransferController(ILogger<PushController> logger)
        {
            this.logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return this.Ok();
        }
    }
}
