namespace CodeReviewAnalyzer.Application.Models.UserDensity;

public class UserReviewerDensity
{
    /// <summary>
    /// How many review threads this user has started.
    /// </summary>
    public required int CommentCount { get; init; }

    public required string UserId { get; init; }

    public required string UserName { get; init; }

    /// <summary>
    /// Reference date. Consider only the month information.
    /// </summary>
    public required DateOnly ReferenceDate { get; init; }
}
