namespace CodeReviewAnalyzer.Application.Models.PullRequestReport;

public class PullRequestOutlier
{
    public required string OutlierField { get; init; }

    public required int OutlierValue { get; init; }

    public required string Url { get; init; }
}
