using CodeReviewInsight.Application.Models.PullRequestReport;
using CodeReviewInsight.Application.Models.UserDensity;

namespace CodeReviewInsight.Application.Reports;

public interface IReport
{
    Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(
        ReportFilter filter);

    Task<IEnumerable<UserReviewerDensity>> GetUserReviewerDensity(
        ReportFilter filter);

    Task<IEnumerable<PullRequestOutlier>> GetPullRequestOutlier(
        ReportFilter filter);
}
