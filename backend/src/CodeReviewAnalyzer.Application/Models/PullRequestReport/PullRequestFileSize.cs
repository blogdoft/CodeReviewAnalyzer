namespace CodeReviewAnalyzer.Application.Models.PullRequestReport;

public class PullRequestFileSize
{
    public required int MeanFileCount { get; init; }

    public required int MaxFileCount { get; init; }

    public required int MinFileCount { get; init; }

    public required DateOnly ReferenceDate { get; init; }
}
