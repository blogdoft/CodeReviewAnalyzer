namespace CodeReviewAnalyzer.Application.Models.PullRequestReport;

public class PullRequestTimeReport
{
    /// <summary>
    /// <para>What is measured: The mean time between the pull request opening and the first
    /// comment made by a revisor.<para>
    /// <pre>Time to start review = First comment date - Pull Request opening date.</pre>
    /// </summary>
    public required IEnumerable<TimeIndex> MeanTimeToStartReview { get; init; }

    /// <summary>
    /// <para>What is measured: The mean time until the Pull Request opening
    /// until it's final approval.</para>
    /// <pre>Mean time to approval = Last approval date - Pull Request opening date</pre>
    /// </summary>
    public required IEnumerable<TimeIndex> MeanTimeToApprove { get; init; }

    /// <summary>
    /// <para>What is measured: The mean time that a Pull Request takes from
    /// being opened to be merged to develop.</para>
    /// <pre>Mean time to merge = Pull Request Merge data - Pull Request opening date</pre>
    /// </summary>
    public required IEnumerable<TimeIndex> MeanTimeToMerge { get; init; }

    /// <summary>
    /// How many pull requests was closed in a period.
    /// </summary>
    public required IEnumerable<TimeIndex> PullRequestCount { get; init; }

    /// <summary>
    /// How many pull requests was closed without any comment in a period.
    /// </summary>
    public required IEnumerable<TimeIndex> ApprovedOnFirstAttempt { get; init; }

    /// <summary>
    /// <para>What is measured: Avg, Max and Min files number by Pull request</para>
    /// </summary>/
    public required IEnumerable<PullRequestFileSize> PullRequestSize { get; init; }
}
