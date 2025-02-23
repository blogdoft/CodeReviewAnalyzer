namespace CodeReviewAnalyzer.Application.Models.PullRequestReport;

public class ReportFilter
{
    /// <summary>
    /// The begin of data range query.
    /// </summary>
    /// <example>2024-01-01</example>
    public DateOnly From { get; set; } = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);

    /// <summary>
    /// The end of data range query.
    /// </summary>
    /// <example>2024-02-28</example>
    public DateOnly To { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddMonths(1).AddTicks(-1));

    /// <summary>
    /// Consider only repositories assigned to this team during KPI evaluation.
    /// </summary>
    public string? RepoTeamId { get; set; }

    /// <summary>
    /// Consider only Users assigned to this team during KPI evaluation.
    /// </summary>
    public string? UserTeamId { get; set; }
}
