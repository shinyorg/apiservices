using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push
{
    public interface IPushManager
    {
        Task Send(Notification notification, PushFilter? filter);
        Task<IEnumerable<PushRegistration>> GetRegistrations(PushFilter? filter);

        Task Register(PushRegistration registration);
        Task UnRegister(PushPlatforms platform, string deviceToken);
        Task UnRegisterByUser(string userId);
    }
}
