namespace Shiny.Contracts;

public record PagedDataRequest(
    int Page,
    int Size,
    OrderBy[]? Ordering = null,
    bool IncludeTotalCount = true
);

public record OrderBy(
    string Property,
    bool Asc = true
);


public record PagedDataList<T>(
    IList<T> Results,
    int CurrentPage,
    int TotalCount,
    int TotalPages
)
{
    public bool HasPrevious => this.CurrentPage > 1;
    public bool HasNext => this.TotalPages > this.CurrentPage;
};