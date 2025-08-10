using System.Diagnostics;

namespace CodeReviewAnalyzer.Application.Models.PagingModels;

public class PageReturn<T>(T data, int totalItem)
{
    public T Data { get; init; } = data;

    public int TotalItem { get; init; } = totalItem;

    public static PageReturn<T> From<TSource>(
        PageReturn<TSource> source,
        Func<T> map)
    {
        var mapped = map();
        return new PageReturn<T>(mapped, source.TotalItem);
    }
}
