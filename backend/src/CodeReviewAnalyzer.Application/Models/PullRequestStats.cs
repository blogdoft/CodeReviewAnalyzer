namespace CodeReviewAnalyzer.Application.Models;

public class PullRequestStats
{
    public required string ExternalId { get; init; }

    public required string Title { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime ClosedAt { get; init; }

    public DateTime? FirstCommentDate { get; init; }

    public int FirstCommentWaitingMinutes { get; init; }

    public int RevisionWaitingTimeMinutes { get; init; }

    public int MergeWaitingTimeMinutes { get; init; }

    public string? MergeMode { get; init; }

    public int FileCount { get; init; }

    public int ThreadCount { get; init; }
}
