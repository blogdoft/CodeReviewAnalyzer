using CodeReviewAnalyzer.Application.Models.PullRequestReport;

namespace CodeReviewAnalyzer.Application.Reports;

public interface IReport
{
    public Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(DateOnly from, DateOnly to);
}
