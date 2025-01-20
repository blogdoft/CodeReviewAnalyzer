using CodeReviewAnalyzer.Application.Services;

namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class PullRequest(WorkingHourCalculator workingHourCalculator)
{
    private readonly WorkingHourCalculator _workingHourCalculator = workingHourCalculator;

    public required int Id { get; init; }

    public required string Title { get; init; }

    public required string RepositoryName { get; init; }

    public required DateTime CreationDate { get; init; }

    public required DateTime ClosedDate { get; init; }

    public required string Url { get; init; }

    public TimeSpan PlainWaitingTime => ClosedDate.Subtract(CreationDate);

    public TimeSpan RevisionWaitingTime => Comments.Any()
        ? _workingHourCalculator.Calculate(
            createdAt: Comments
                .Select(c => c.CommentDate)
                .DefaultIfEmpty(DateTime.MinValue)
                .Min(),
            closedAt: Comments
                .Select(c => c.ResolvedDate)
                .DefaultIfEmpty(DateTime.MinValue)
                .Max())
        : TimeSpan.Zero;

    public TimeSpan MergeWaitingTime => _workingHourCalculator.Calculate(
        createdAt: CreationDate,
        closedAt: ClosedDate);

    public TimeSpan FirstCommentWaitingTime => Comments.Any()
        ? _workingHourCalculator.Calculate(
            createdAt: CreationDate,
            closedAt: Comments
                .Select(c => c.CommentDate)
                .DefaultIfEmpty(DateTime.MinValue)
                .Min())
        : TimeSpan.Zero;

    public required User CreatedBy { get; init; }

    public required int FileCount { get; init; }

    public required int ThreadCount { get; init; } = 0;

    public required DateTime FirstCommentDate { get; init; }

    public required DateTime LastApprovalDate { get; init; }

    public required string MergeMode { get; init; }

    public required IEnumerable<User> Reviewers { get; init; }

    public required IEnumerable<PrComments> Comments { get; init; }
}
