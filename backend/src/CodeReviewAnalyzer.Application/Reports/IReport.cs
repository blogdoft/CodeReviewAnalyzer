using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.UserDensity;

namespace CodeReviewAnalyzer.Application.Reports;

public interface IReport
{
    Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(
        ReportFilter filter);

    Task<IEnumerable<UserReviewerDensity>> GetUserReviewerDensity(
        ReportFilter filter);

    Task<IEnumerable<PullRequestOutlier>> GetPullRequestOutlier(
        ReportFilter filter);
}
