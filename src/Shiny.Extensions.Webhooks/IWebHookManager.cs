using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shiny.Extensions.Webhooks.Infrastructure;

namespace Shiny.Extensions.Webhooks;


public interface IWebHookManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    Task<IEnumerable<WebHookRegistration>> GetRegistrations(string eventName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    Task Subscribe(WebHookRegistration registration);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="registrationId"></param>
    /// <returns></returns>
    Task UnSubscribe(Guid registrationId);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="registration"></param>
    /// <param name="args"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task<T?> Request<T>(WebHookRegistration registration, object? args, CancellationToken cancelToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="args"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task<SendBatchResult> Send(string eventName, object? args, CancellationToken cancelToken = default);
}
