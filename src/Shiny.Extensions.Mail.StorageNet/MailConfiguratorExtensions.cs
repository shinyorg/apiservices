using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Mail.Impl;
using Storage.Net;
using Storage.Net.Blobs;


namespace Shiny.Extensions.Mail
{
    public static class MailConfiguratorExtensions
    {
        public static MailConfigurator UseStorageNetTemplateLoader(this MailConfigurator cfg, IBlobStorage blobStorage)
        {
            cfg.Services.AddSingleton(blobStorage);
            cfg.Services.AddSingleton<ITemplateLoader, StorageTemplateLoader>();
            return cfg;
        }


        public static MailConfigurator UseStorageNetTemplateLoader(this MailConfigurator cfg, string connectionString)
        {
            var blobStore = StorageFactory.Blobs.FromConnectionString(connectionString);
            return cfg.UseStorageNetTemplateLoader(blobStore);
        }
    }
}
