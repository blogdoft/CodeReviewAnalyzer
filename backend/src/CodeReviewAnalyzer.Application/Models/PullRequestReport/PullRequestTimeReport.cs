namespace CodeReviewAnalyzer.Application.Models.PullRequestReport;

public class PullRequestTimeReport
{
    public required IEnumerable<TimeIndex> MeanTimeOpenToApproval { get; init; }

    public required IEnumerable<TimeIndex> MeanTimeToStartReview { get; init; }

    public required IEnumerable<TimeIndex> MeanReviewingTime { get; init; }

    public required IEnumerable<PullRequestFileSize> PullRequestSize { get; init; }
}
