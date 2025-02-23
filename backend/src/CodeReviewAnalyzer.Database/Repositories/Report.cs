using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.UserDensity;
using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Database.Contexts;
using CodeReviewAnalyzer.Database.Services;

namespace CodeReviewAnalyzer.Database.Repositories;

public class Report(IDatabaseFacade databaseFacade) : IReport
{
    private const string ReviewerDensity =
        """
            with reviewers as (
                select distinct pr."ID" as "PR_ID"
                    , u."ID" as "UserId"
                    , u."NAME" as "UserName"
                    , date_trunc('month', pr."CLOSED_DATE" ) as "ReferenceDate"
                from "PULL_REQUEST" pr 
                    join "PULL_REQUEST_COMMENTS" prc on prc."PULL_REQUEST_ID"  = pr."ID" and prc."COMMENT_INDEX" = 1 
                    join "USERS" u on u."ID" = prc."USER_ID" and u."ACTIVE"
                where pr."CLOSED_DATE" between @From and @To)
            select count(r."PR_ID") as "CommentCount"
                , r."UserId" 
                , r."UserName"
                , r."ReferenceDate"
            from reviewers r
            group by 2, 3, 4
            order by 4, 1 desc

        """;

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
            MeanTimeOpenToApproval = meanTimeToApprove,
            MeanTimeToStartReview = meanTimeToReview,
            MeanTimeToMerge = meanTimeToMerge,
            PullRequestCount = pullRequestCount,
            PullRequestWithoutCommentCount = approvedOnFirstAttempt,
            PullRequestSize = pullRequestStats,
        };
    }

    public async Task<IEnumerable<UserReviewerDensity>> GetUserReviewerDensity(
        ReportFilter filter)
    {
        var userDensity = await databaseFacade.QueryAsync<UserReviewerDensity>(
            ReviewerDensity,
            new
            {
                filter.From,
                filter.To,
            });

        return userDensity ?? [];
    }
}
