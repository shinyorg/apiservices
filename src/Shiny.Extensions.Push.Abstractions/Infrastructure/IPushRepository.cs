namespace Shiny.Extensions.Push.Infrastructure;


public interface IPushRepository
{
    /// <summary>
    /// Gets list of registrations based on the criteria (all if filter is null)
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task<IList<PushRegistration>> Get(Filter? filter, CancellationToken cancelToken = default);

    /// <summary>
    /// Saves/Updates a registration
    /// </summary>
    /// <param name="reg"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task Save(PushRegistration reg, CancellationToken cancelToken = default);

    /// <summary>
    /// Removes any registrations within the criteria
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task Remove(Filter filter, CancellationToken cancelToken = default);

    /// <summary>
    /// Remove specific registrations
    /// </summary>
    /// <param name="pushRegistrations"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    Task RemoveBatch(PushRegistration[] pushRegistrations, CancellationToken cancelToken = default);
}

