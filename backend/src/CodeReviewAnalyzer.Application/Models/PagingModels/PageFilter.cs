namespace CodeReviewAnalyzer.Application.Models.PagingModels;

public readonly struct PageFilter
{
    public int Page { get; init; }

    public int Size { get; init; }

    public string? Order { get; init; }
}
