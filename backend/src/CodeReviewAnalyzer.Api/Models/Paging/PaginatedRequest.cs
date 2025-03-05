using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Api.Models.Paging;

public class PaginatedRequest
{
    /// <summary>
    /// Gets the page number to be retrieved. Defaults to 0.
    /// </summary>
    /// <example>0</example>
    public int Page { get; init; } = 0;

    /// <summary>
    /// Gets the number of items to be retrieved per page. Defaults to 10.
    /// </summary>
    /// <example>15</example>
    public int Size { get; init; } = 10;

    /// <summary>
    /// Gets the order in which the data should be sorted.
    /// </summary>
    /// <example>name ASC</example>
    public string? Order { get; init; }

    internal PageFilter ToPageFilter() => new()
    {
        Page = Page,
        Size = Size,
        Order = Order,
    };
}
