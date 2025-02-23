using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.UserDensity;
using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Database.Contexts;

namespace CodeReviewAnalyzer.Database.Repositories;

public class Report(IDatabaseFacade databaseFacade) : IReport
{
    private const string TimeUntilApproval =
        """
            -- Review time: Start review mean time
            select avg(pr."FIRST_COMMENT_WAITING_TIME_MINUTES") as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID"  and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
              and pr."FIRST_COMMENT_DATE" <> pr."CREATION_DATE"
            group by 2
            order by 2 asc
            
            ;
            
            --Mean time to approval
            select (extract(epoch from  avg(pr."LAST_APPROVAL_DATE" - pr."CREATION_DATE")) / 60 )::int as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID"  and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
            group by 2
            order by 2 asc

            ;

            -- Mean time to merge
            select avg(pr."MERGE_WAITING_TIME_MINUTES") as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE") as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID"  and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
            group by 2
            order by 2

            ;

            -- Pull Request count
            select count(1) as PeriodInMinutes
                , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr  
                join "USERS" u on u."ID" = pr."CREATED_BY_ID"  and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
            group by 2
            order by 2 asc
            
            ;

            -- No Commented PullRequests
            select count(1) as PeriodInMinutes
                , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr  
                join "USERS" u on u."ID" = pr."CREATED_BY_ID"  and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
              and pr."THREAD_COUNT" = 0
            group by 2
            order by 2 asc

            ;

            -- Pull Request counters
            select avg(pr."FILE_COUNT") as "MeanFileCount"
                , max(pr."FILE_COUNT") as "MaxFileCount"
                , min(pr."FILE_COUNT") as "MinFileCount"
                , count(pr."ID") as "PrCount"
                , date_trunc('month', pr."CLOSED_DATE" ) as "ReferenceDate"
            from "PULL_REQUEST" pr
            join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
            group by "ReferenceDate"
            order by "ReferenceDate" asc
                       
        """;

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
                where pr."CLOSED_DATE" between @from  and @to)
            select count(r."PR_ID") as "CommentCount"
                , r."UserId" 
                , r."UserName"
                , r."ReferenceDate"
            from reviewers r
            group by 2, 3, 4
            order by 4, 1 desc

        """;

    public async Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(
        DateOnly from,
        DateOnly to)
    {
        using var resultSets = await databaseFacade.QueryMultipleAsync(
            TimeUntilApproval,
            new
            {
                from,
                to,
            });
        var timeUntilApprove = await resultSets.ReadAsync<TimeIndex>();
        var startReviewMeanTime = await resultSets.ReadAsync<TimeIndex>();
        var meanTimeToMerge = await resultSets.ReadAsync<TimeIndex>();
        var pullRequestCount = await resultSets.ReadAsync<TimeIndex>();
        var nonCommentedPullRequest = await resultSets.ReadAsync<TimeIndex>();
        var pullRequestCounters = await resultSets.ReadAsync<PullRequestFileSize>();

        return new()
        {
            MeanTimeOpenToApproval = timeUntilApprove,
            MeanTimeToStartReview = startReviewMeanTime,
            MeanTimeToMerge = meanTimeToMerge,
            PullRequestCount = pullRequestCount,
            PullRequestWithoutCommentCount = nonCommentedPullRequest,
            PullRequestSize = pullRequestCounters,
        };
    }

    public async Task<IEnumerable<UserReviewerDensity>> GetUserReviewerDensity(
        DateOnly from,
        DateOnly to)
    {
        var userDensity = await databaseFacade.QueryAsync<UserReviewerDensity>(
            ReviewerDensity,
            new
            {
                from,
                to,
            });

        return userDensity ?? [];
    }
}
