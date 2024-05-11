namespace Shiny.Contracts;

public record DateRange(DateOnly Start, DateOnly End);
public record TimeRange(TimeOnly Start, TimeOnly End);
public record DateTimeRange(DateTimeOffset Start, DateTimeOffset End);