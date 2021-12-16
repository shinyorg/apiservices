using Refit;
using SampleWeb.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SampleMobile
{
    public interface ISampleApi
    {
        [Post("/mail/send/{templateName}")]
        Task SendMail(string templateName, [Body(BodySerializationMethod.Serialized)] SendMail mail);


        [Post("/mail/parse/{templateName}")]
        Task<string> TestMailParse(string templateName, [Body(BodySerializationMethod.Serialized)] SendMail mail);


        [Get("/storage/providers")]
        Task<string[]> GetFileProviders();


        [Post("/storage/list")]
        Task<List<StorageItem>> GetFileList([Body(BodySerializationMethod.Serialized)] ListStorage filter);


        [Post("/storage/viewfile")]
        Task<string> ViewFile([Body(BodySerializationMethod.Serialized)] ListStorage filter);


        [Post("/push/registrations")]
        Task<List<Registration>> GetRegistrations([Body(BodySerializationMethod.Serialized)] Registration filter);


        [Post("/push/send")]
        Task Send([Body(BodySerializationMethod.Serialized)] Notification notification);


        [Post("/push/register")]
        Task RegisterPush([Body(BodySerializationMethod.Serialized)] Registration register);


        [Delete("/push/unregister/{platform}/{deviceToken}")]
        Task UnRegisterPush(string platform, string deviceToken);
    }
}
