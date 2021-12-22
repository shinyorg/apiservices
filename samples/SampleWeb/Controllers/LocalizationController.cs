using Microsoft.AspNetCore.Mvc;
using Shiny.Extensions.Localization;

using System.Globalization;

namespace SampleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalizationController : ControllerBase
    {
        readonly ILocalizationManager localizationManager;
        public LocalizationController(ILocalizationManager localizationManager)
            => this.localizationManager = localizationManager;


        [HttpGet("all/{cultureCode?}")]
        public async Task<ActionResult> All(string cultureCode = "en-US")
        {
            var culture = CultureInfo.CreateSpecificCulture(cultureCode);
            var content = this.localizationManager.GetAllSectionsWithKeys(culture);
            return this.Ok(content);
        }


        [HttpGet("section/{section}/{cultureCode?}")]
        public async Task<ActionResult> Section(string section, string cultureCode = "en-US")
        {
            var culture = CultureInfo.CreateSpecificCulture(cultureCode);
            var all = this.localizationManager.GetSection(section).GetValues(culture);
            return this.Ok(all);
        }
    }
}
