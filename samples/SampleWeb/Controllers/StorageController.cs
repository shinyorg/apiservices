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
        public async Task<ActionResult<List<StorageItem>>> List([FromBody] ListStorage contract)
        {
            var provider = this.GetProvider(contract.ProviderName);
            var files = (await provider.GetDirectoryContents(contract.Path))
                .Select(x => new StorageItem
                {
                    Name = x.Name,
                    FullName = x.FullName,
                    IsDirectory = x.IsDirectory()
                })
                .ToList();

            return this.Ok(files);
        }


        [HttpPost("viewfile")]
        public async Task<ActionResult<string>> ViewFile([FromBody] ListStorage contract)
        {
            var provider = this.GetProvider(contract.ProviderName);
            var file = await provider.GetFile(contract.Path);
            if (file == null || !file.Exists)
                return this.NotFound("File not found");

            var content = await file.ReadFileAsString();
            return this.Ok(content);
        }


        IAsyncFileProvider GetProvider(string name)
        {
            foreach (var provider in this.providers)
            {
                var typeName = provider.GetType().Name;
                if (typeName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return provider;

                typeName = typeName.ToLower().Replace("asyncfileprovider", String.Empty);
                if (typeName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return provider;

                if (provider.GetType().FullName!.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                    return provider;
            }
            throw new ArgumentException("No provider found for " + name);
        }
    }
}
