using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shiny.Api.Push.Infrastructure
{
    public interface IRepository
    {
        Task<IEnumerable<PushRegistration>> Get(PushFilter? filter);
        Task Save(PushRegistration reg);
        Task Remove(PushFilter filter);
    }
}
