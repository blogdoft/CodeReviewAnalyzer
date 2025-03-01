using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Api.Models.Paging;

public abstract class PaginatedResponse<TData>
{
    protected PaginatedResponse(
        PageReturn<TData> pageResult,
        PaginatedRequest pageFilter)
    {
        Data = pageResult.Data;
        TotalItems = pageResult.TotalItem;
        CurrentPage = pageFilter.Page;
        TotalPages = TotalItems == 0
            ? 0
            : (TotalItems / pageFilter.Size) + 1;
    }

    public TData Data { get; init; }

    public int CurrentPage { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }
}
