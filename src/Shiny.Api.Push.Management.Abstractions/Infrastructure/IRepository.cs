using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shiny.Api.Push.Management.Infrastructure
{
    public interface IRepository
    {
        Task<IEnumerable<NotificationRegistration>> Get(PushFilter? filter);
        Task Save(NotificationRegistration reg);
        Task Remove(PushFilter filter);
    }
}
