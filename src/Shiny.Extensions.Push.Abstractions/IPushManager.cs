using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push
{
    public interface IPushManager
    {
        Task Send(Notification notification, PushFilter? filter, CancellationToken cancelToken = default);
        Task<IEnumerable<PushRegistration>> GetRegistrations(PushFilter? filter, CancellationToken cancelToken = default);

        Task Register(PushRegistration registration, CancellationToken cancelToken = default);
        Task UnRegister(PushPlatforms platform, string deviceToken, CancellationToken cancelToken = default);
        Task UnRegisterByUser(string userId, CancellationToken cancelToken = default);
    }
}
