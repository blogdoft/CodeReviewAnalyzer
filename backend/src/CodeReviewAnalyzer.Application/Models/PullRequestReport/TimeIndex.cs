namespace CodeReviewAnalyzer.Application.Models.PullRequestReport;

public class TimeIndex
{
    /// <summary>
    /// The measured time - in minutes
    /// </summary>
    public int PeriodInMinutes { get; init; }

    /// <summary>
    /// <para>A reference date to measured time.</para>
    /// <para>Although the value always will be a first month's day, this
    /// property represents the entire month</para>
    /// </summary>
    public DateTime ReferenceDate { get; init; }
}
