using CodeReviewAnalyzer.Application.Models.PullRequestReport;

namespace CodeReviewAnalyzer.Database.Services;

public static class PullRequestInsightReportQueryBuilder
{
    private const string RepoTeamJoin =
        """

            join "TEAM_REPOSITORY" tr on tr.repository_id = pr."REPOSITORY_ID" 
            join "TEAMS" repo_team on repo_team.id = tr.team_id
            
        """;

    private const string UserTeamJoin =
        """
            join "TEAM_USER" tu on tu.user_id = u."ID"
            join "TEAMS" user_team on user_team.id = tu.team_id

        """;

    private const string MeanTimeToReview =
        """
            select avg(pr."FIRST_COMMENT_WAITING_TIME_MINUTES") as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE" 
                {0}
            where pr."CLOSED_DATE" between @From and @To
              -- FirstCommentDate is equal to CreationDate when a pr has no comment.
              and pr."FIRST_COMMENT_DATE" <> pr."CREATION_DATE"
              {1}
            group by 2
            order by 2 asc

        """;

    private const string MeanTimeToMerge =
        """
            -- Mean time to merge
            select avg(pr."MERGE_WAITING_TIME_MINUTES") as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE") as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE" 
                {0}
            where pr."CLOSED_DATE" between @From and @To
            {1}
            group by 2
            order by 2 asc

        """;

    private const string MeanTimeToApprove =
        """
            select (extract(epoch from  avg(pr."LAST_APPROVAL_DATE" - pr."CREATION_DATE")) / 60 )::int as PeriodInMinutes
                 , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr 
                join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE"
                {0}
            where pr."CLOSED_DATE" between @From and @To
                {1}
            group by 2
            order by 2 asc

        """;

    private const string PullRequestCount =
        """
            -- Pull Request count
            select count(1) as PeriodInMinutes
                , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr  
                join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE" 
                {0}
            where pr."CLOSED_DATE" between @From and @To
            {1}
            group by 2
            order by 2 asc
        
        """;

    private const string ApprovedOnFirstAttempt =
        """
            select count(1) as PeriodInMinutes
                , date_trunc('month', pr."CLOSED_DATE" ) as ReferenceDate
            from "PULL_REQUEST" pr  
                join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE" 
                {0}
            where pr."CLOSED_DATE" between @From and @To
              and pr."THREAD_COUNT" = 0
              {1}
            group by 2
            order by 2 asc


        """;

    private const string PullRequestStats =
        """
            -- Pull Request counters
            select avg(pr."FILE_COUNT") as "MeanFileCount"
                , max(pr."FILE_COUNT") as "MaxFileCount"
                , min(pr."FILE_COUNT") as "MinFileCount"
                , count(pr."ID") as "PrCount"
                , date_trunc('month', pr."CLOSED_DATE" ) as "ReferenceDate"
            from "PULL_REQUEST" pr
                join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE" 
                {0}
            where pr."CLOSED_DATE" between @From and @To
            {1}
            group by "ReferenceDate"
            order by "ReferenceDate" asc

        """;

    public static string BuildPullRequestSql(ReportFilter filter)
    {
        // Joins opcionais para time de repositórios
        var repoTeamJoin = !string.IsNullOrEmpty(filter.RepoTeamId)
            ? RepoTeamJoin
            : string.Empty;

        // Joins opcionais para time de usuários
        var userTeamJoin = !string.IsNullOrEmpty(filter.UserTeamId)
            ? UserTeamJoin
            : string.Empty;

        // Condições opcionais no WHERE
        var repoTeamCondition = !string.IsNullOrEmpty(filter.RepoTeamId)
            ? " and repo_team.external_id = @repoTeamId"
            : string.Empty;

        var userTeamCondition = !string.IsNullOrEmpty(filter.UserTeamId)
            ? " and user_team.external_id = @userTeamId"
            : string.Empty;

        // Exemplo: Query de "FirstCommentWaitingTime"
        var meanTimeToReview = string.Format(
            MeanTimeToReview,
            userTeamJoin + repoTeamJoin,
            userTeamCondition + repoTeamCondition);
        var meanTimeToApprove = string.Format(
            MeanTimeToApprove,
            userTeamJoin + repoTeamJoin,
            userTeamCondition + repoTeamCondition);
        var meanTimeToMerge = string.Format(
            MeanTimeToMerge,
            userTeamJoin + repoTeamJoin,
            userTeamCondition + repoTeamCondition);
        var pullRequestCount = string.Format(
            PullRequestCount,
            userTeamJoin + repoTeamJoin,
            userTeamCondition + repoTeamCondition);
        var approvedOnFirstAttempt = string.Format(
            ApprovedOnFirstAttempt,
            userTeamJoin + repoTeamJoin,
            userTeamCondition + repoTeamCondition);
        var pullRequestStats = string.Format(
            PullRequestStats,
            userTeamJoin + repoTeamJoin,
            userTeamCondition + repoTeamCondition);

        return string.Join(
            ";\n\n",
            meanTimeToApprove,
            meanTimeToReview,
            meanTimeToMerge,
            pullRequestCount,
            approvedOnFirstAttempt,
            pullRequestStats);
    }
}
