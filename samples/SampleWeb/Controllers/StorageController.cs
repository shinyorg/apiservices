using Microsoft.AspNetCore.Mvc;
using SampleWeb.Contracts;
using Shiny.Storage;


namespace SampleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        readonly IEnumerable<IAsyncFileProvider> providers;
        public StorageController(IEnumerable<IAsyncFileProvider> providers)
            => this.providers = providers;


        [HttpGet("providers")]
        public ActionResult<string[]> Providers()
            => this.Ok(this.providers.Select(x => x.GetType().FullName).ToArray());


        [HttpPost("list")]
        public async Task<ActionResult> List([FromBody] ListStorage contract)
        {
            var provider = this.GetProvider(contract.ProviderName);
            var files = await provider.GetDirectoryContents(contract.Path);
            return this.Ok(files);
        }


        IAsyncFileProvider GetProvider(string name)
        {
            foreach (var provider in this.providers)
            {
                var typeName = provider.GetType().Name;
                if (typeName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return provider;

                typeName = typeName.ToLower().Replace("AsyncFileProvider", String.Empty);
                if (typeName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return provider;

                if (provider.GetType().FullName.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                    return provider;
            }
            throw new ArgumentException("No provider found for " + name);
        }
    }
}
