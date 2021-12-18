using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push.Infrastructure
{
    public interface IRepository
    {
        Task<IEnumerable<PushRegistration>> Get(PushFilter? filter, CancellationToken cancelToken);
        Task Save(PushRegistration reg, CancellationToken cancelToken);
        Task Remove(PushFilter filter, CancellationToken cancelToken);

        Task RemoveBatch(PushRegistration[] pushRegistrations, CancellationToken cancelToken);
    }
}
