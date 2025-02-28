using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.UserDensity;
using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Database.Contexts;
using CodeReviewAnalyzer.Database.Services;

namespace CodeReviewAnalyzer.Database.Repositories;

public class Report(IDatabaseFacade databaseFacade) : IReport
{
    public Task<IEnumerable<PullRequestOutlier>> GetPullRequestOutlier(ReportFilter filter)
    {
        var sql = PullRequestInsightReportQueryBuilder.BuildOutlier(filter);
        return databaseFacade.QueryAsync<PullRequestOutlier>(sql, new
        {
            filter.From,
            filter.To,
            filter.RepoTeamId,
            filter.UserTeamId,
        });
    }

    public async Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(
        ReportFilter filter)
    {
        // TODO: Discover a way to maintain the query order inside Query Builder
        // and resultSets.ReadAsync.
        var sql = PullRequestInsightReportQueryBuilder.BuildPullRequestSql(filter);

        using var resultSets = await databaseFacade.QueryMultipleAsync(
            sql,
            new
            {
                filter.From,
                filter.To,
                repoTeamId = filter.RepoTeamId,
                userTeamId = filter.UserTeamId,
            });

        var meanTimeToApprove = await resultSets.ReadAsync<TimeIndex>();
        var meanTimeToReview = await resultSets.ReadAsync<TimeIndex>();
        var meanTimeToMerge = await resultSets.ReadAsync<TimeIndex>();
        var pullRequestCount = await resultSets.ReadAsync<TimeIndex>();
        var approvedOnFirstAttempt = await resultSets.ReadAsync<TimeIndex>();
        var pullRequestStats = await resultSets.ReadAsync<PullRequestFileSize>();

        return new()
        {
            MeanTimeToApprove = meanTimeToApprove,
            MeanTimeToStartReview = meanTimeToReview,
            MeanTimeToMerge = meanTimeToMerge,
            PullRequestCount = pullRequestCount,
            ApprovedOnFirstAttempt = approvedOnFirstAttempt,
            PullRequestSize = pullRequestStats,
        };
    }

    public async Task<IEnumerable<UserReviewerDensity>> GetUserReviewerDensity(
        ReportFilter filter)
    {
        var sql = PullRequestInsightReportQueryBuilder.BuildDeveloperDensity(filter);
        var userDensity = await databaseFacade.QueryAsync<UserReviewerDensity>(
            sql,
            new
            {
                filter.From,
                filter.To,
                filter.RepoTeamId,
                filter.UserTeamId,
            });

        return userDensity ?? [];
    }
}
