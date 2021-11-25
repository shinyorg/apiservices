using Refit;
using SampleWeb.Contracts;
using System.Threading.Tasks;


namespace SampleMobile
{
    public interface ISampleApi
    {
        [Post("/push/send")]
        Task Send([Body(BodySerializationMethod.Serialized)] Notification notification);

        [Post("/push/register")]
        Task Register([Body(BodySerializationMethod.Serialized)] Registration register);

        [Post("/push/unregister")]
        Task UnRegister([Body(BodySerializationMethod.Serialized)] Registration register);
    }
}
