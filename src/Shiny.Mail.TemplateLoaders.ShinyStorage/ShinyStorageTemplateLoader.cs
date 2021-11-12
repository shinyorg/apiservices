using Shiny.Storage;

namespace Shiny.Mail.TemplateLoaders.ShinyStorage
{
    public class ShinyStorageTemplateLoader : ITemplateLoader
    {
        readonly IStorage storage;


        public ShinyStorageTemplateLoader(IStorage storage)
        {
            this.storage = storage;
        }


        public Task<string> Load(string templateName)
        {
            throw new NotImplementedException();
        }
    }
}