using Storage.Net.Blobs;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class StorageTemplateLoader : ITemplateLoader
    {
        readonly IBlobStorage storage;
        readonly string rootPath;
        readonly string? extension;


        public StorageTemplateLoader(IBlobStorage storage, string rootPath, string? extension = null)
        {
            this.storage = storage;
            this.rootPath = rootPath;
            this.extension = extension;
        }


        public async Task<string> Load(string templateName, CultureInfo? culture = null, CancellationToken cancellationToken = default)
        {
            // TODO: factor in culture /w fallback
            var fullPath = this.rootPath + templateName + this.extension;

            using (var stream = await this.storage.OpenReadAsync(fullPath, cancellationToken).ConfigureAwait(false))
                using (var sr = new StreamReader(stream))
                    return await sr.ReadToEndAsync();
        }
    }
}