namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class PullRequest
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required CodeRepository Repository { get; init; }

    public required DateTime CreationDate { get; init; }

    public required DateTime ClosedDate { get; init; }

    public required string Url { get; init; }

    public TimeSpan PlainWaitingTime => ClosedDate.Subtract(CreationDate);

    public TimeSpan RevisionWaitingTime => Comments.Any()
        ? Comments
            .Select(c => c.ResolvedDate)
            .DefaultIfEmpty(DateTime.MinValue)
            .Max() - Comments
            .Select(c => c.CommentDate)
            .DefaultIfEmpty(DateTime.MinValue)
            .Min()
        : TimeSpan.Zero;

    public TimeSpan MergeWaitingTime => ClosedDate - CreationDate;

    public TimeSpan FirstCommentWaitingTime => Comments.Any()
        ? Comments
                .Select(c => c.CommentDate)
                .DefaultIfEmpty(DateTime.MinValue)
                .Min() - CreationDate
        : TimeSpan.Zero;

    public required User CreatedBy { get; init; }

    public required int FileCount { get; init; }

    public required int ThreadCount { get; init; } = 0;

    public required DateTime FirstCommentDate { get; init; }

    public required DateTime LastApprovalDate { get; init; }

    public required string MergeMode { get; init; }

    public required IEnumerable<Reviewer> Reviewers { get; init; }

    public required IEnumerable<PrComments> Comments { get; init; }
}
