using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Database.Contexts;

namespace CodeReviewAnalyzer.Database.Repositories;

public class Report(IDatabaseFacade databaseFacade) : IReport
{
    private const string TimeUntilApproval =
        """
            --Review Time Open - Approval
            select (extract(epoch from  avg(pr."LAST_APPROVAL_DATE" - pr."CREATION_DATE")) / 60 )::int as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID"  and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
            group by 2
            order by 2 asc
            ;
            
            -- Review time: Start review mean time
            select (avg(pr."FIRST_COMMENT_WAITING_TIME_MINUTES") / 60)::int as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID"  and u."ACTIVE" 
            where pr."CLOSED_DATE" between @from and @to
              and pr."FIRST_COMMENT_DATE" <> pr."CREATION_DATE"
            group by 2
            order by 2 asc            

        """;

    public async Task<PullRequestTimeReport> GetPullRequestTimeReportAsync(DateOnly from, DateOnly to)
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

        return new()
        {
            MeanTimeOpenToApproval = timeUntilApprove,
            MeanTimeToStartReview = startReviewMeanTime,
            MeanReviewingTime =[],
            PullRequestSize =[],
        };
    }
}
