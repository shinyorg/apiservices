using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Extensions.Webhooks.Infrastructure;


public interface IRunner
{
    Task<T?> Request<T>(WebHookRegistration registration, object? args, CancellationToken cancelToken);
    Task Send(WebHookRegistration registration, object? args, CancellationToken cancelToken);
}
