using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shiny.Contracts;

namespace Shiny;


public static class QueryableExtensions
{
    public static async Task<PagedDataList<TResult>> ToPagedListAsync<TData, TResult>(
        this IQueryable<TData> query,
        PagedDataRequest request,
        Expression<Func<TData, TResult>> selectExpression,
        CancellationToken cancellationToken
    ) where TData: class
    {
        var totalCount = -1;
        var totalPages = 0;

        if (request.IncludeTotalCount)
        {
            totalCount = await query.CountAsync(cancellationToken);
            if (totalCount > 0)
                totalPages = (int)Math. Ceiling((double)totalCount / request.Size);
        }

        var resultQuery = query.Select(selectExpression);
        if (request.Ordering != null)
        {
            foreach (var orderBy in request.Ordering)
            {
                resultQuery = resultQuery.OrderByDynamic(orderBy);
            }
        }

        var results = await query
            .Select(selectExpression)
            .Skip(request.Page * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return new PagedDataList<TResult>(
            results,
            request.Page,
            totalCount,
            totalPages
        );
    }


    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, OrderBy order)
    {
        var queryElementTypeParam = Expression.Parameter(typeof(T));
        var memberAccess = Expression.PropertyOrField(queryElementTypeParam, order.Property);
        var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

        // TODO: thenBy
        var orderBy = Expression.Call(
            typeof(Queryable),
            order.Asc ? "OrderBy" : "OrderByDescending",
            new Type[] { typeof(T), memberAccess.Type },
            query.Expression,
            Expression.Quote(keySelector)
        );
        return query.Provider.CreateQuery<T>(orderBy);
    }


    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        var list = new List<T>();
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            list.Add(element);
        }
        return list;
    }
}