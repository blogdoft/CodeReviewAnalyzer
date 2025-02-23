using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.UserDensity;

namespace CodeReviewAnalyzer.Application.Reports;

public interface IReport
{
    public Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(
        ReportFilter filter);

    public Task<IEnumerable<UserReviewerDensity>> GetUserReviewerDensity(
        ReportFilter filter);
}
