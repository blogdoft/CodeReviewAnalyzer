namespace CodeReviewAnalyzer.Application.Models.PagingModels;

public class PageReturn<T>(T data, int totalItem)
{
    public T Data { get; init; } = data;

    public int TotalItem { get; init; } = totalItem;
}
