using Shiny.Storage;

namespace Shiny.Mail.Impl
{
    public class ShinyStorageTemplateLoader : ITemplateLoader
    {
        readonly IAsyncFileProvider storage;
        readonly string rootPath;
        readonly string? extension;


        public ShinyStorageTemplateLoader(IAsyncFileProvider storage, string rootPath, string? extension = null)
        {
            this.storage = storage;
            this.rootPath = rootPath;
            this.extension = extension;
        }


        public async Task<string> Load(string templateName)
        {
            var fullPath = this.rootPath + templateName + this.extension;

            var file = await this.storage
                .GetFile(fullPath)
                .ConfigureAwait(false);

            if (!file.Exists)
                throw new ArgumentException($"Template file {fullPath} does not exist");

            using (var stream = await file.OpenStream(false))
                using (var sr = new StreamReader(stream))
                    return await sr.ReadToEndAsync();
        }
    }
}