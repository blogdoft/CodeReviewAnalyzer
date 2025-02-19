using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.UserDensity;

namespace CodeReviewAnalyzer.Application.Reports;

public interface IReport
{
    public Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(DateOnly from, DateOnly to);

    public Task<IEnumerable<UserReviewerDensity>> GetUserReviewerDensity(DateOnly from, DateOnly to);
}
