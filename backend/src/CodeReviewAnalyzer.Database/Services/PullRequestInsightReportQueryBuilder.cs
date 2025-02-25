using CodeReviewAnalyzer.Application.Models.PullRequestReport;

namespace CodeReviewAnalyzer.Database.Services;

public static class PullRequestInsightReportQueryBuilder
{
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

    private const string UserReviewerDensitySql =
        """
            with reviewers as (
                select distinct pr."ID" as "PR_ID"
                    , u."ID" as "UserId"
                    , u."NAME" as "UserName"
                    , date_trunc('month', pr."CLOSED_DATE" ) as "ReferenceDate"
                from "PULL_REQUEST" pr
                    join "PULL_REQUEST_COMMENTS" prc on prc."PULL_REQUEST_ID"  = pr."ID" and prc."COMMENT_INDEX" = 1 
                    join "USERS" u on u."ID" = prc."USER_ID" and u."ACTIVE"
                    {0}
                where pr."CLOSED_DATE" between @From and @To
                  {1}  
            )
            select count(r."PR_ID") as "CommentCount"
                , r."UserId" 
                , r."UserName"
                , r."ReferenceDate"
            from reviewers r
            group by 2, 3, 4
            order by 4, 1 desc

        """;

    public static string BuildPullRequestSql(ReportFilter filter)
    {
        var sqlFilter = new RepoSqlFilter(filter);

        // Exemplo: Query de "FirstCommentWaitingTime"
        var meanTimeToReview = Merge(MeanTimeToReview, sqlFilter);
        var meanTimeToApprove = Merge(MeanTimeToApprove, sqlFilter);
        var meanTimeToMerge = Merge(MeanTimeToMerge, sqlFilter);
        var pullRequestCount = Merge(PullRequestCount, sqlFilter);
        var approvedOnFirstAttempt = Merge(ApprovedOnFirstAttempt, sqlFilter);
        var pullRequestStats = Merge(PullRequestStats, sqlFilter);

        return string.Join(
            ";\n\n",
            meanTimeToApprove,
            meanTimeToReview,
            meanTimeToMerge,
            pullRequestCount,
            approvedOnFirstAttempt,
            pullRequestStats);
    }

    public static string BuildDeveloperDensity(ReportFilter filter)
    {
        var sqlFilter = new RepoSqlFilter(filter);

        // Exemplo: Query de "FirstCommentWaitingTime"
        return Merge(UserReviewerDensitySql, sqlFilter);
    }

    private static string Merge(string sql, RepoSqlFilter sqlFilter) => string.Format(
        sql,
        sqlFilter.UserTeamJoin + sqlFilter.RepoTeamJoin,
        sqlFilter.UserTeamWhere + sqlFilter.RepoTeamWhere);

    private sealed class RepoSqlFilter(ReportFilter filter)
    {
        private const string RepoTeamJoins =
            """

            join "TEAM_REPOSITORY" tr on tr.repository_id = pr."REPOSITORY_ID" 
            join "TEAMS" repo_team on repo_team.id = tr.team_id
            
        """;

        private const string UserTeamJoins =
            """
            join "TEAM_USER" tu on tu.user_id = u."ID"
            join "TEAMS" user_team on user_team.id = tu.team_id

        """;

        private const string RepoTeamCondition =
            """
                and repo_team.external_id = @repoTeamId

            """;

        private const string RepoUserCondition =
            """
                and user_team.external_id = @userTeamId

            """;

        public string RepoTeamJoin { get; } =
            !string.IsNullOrEmpty(filter.RepoTeamId)
                ? RepoTeamJoins
                : string.Empty;

        public string RepoTeamWhere { get; } =
            !string.IsNullOrEmpty(filter.RepoTeamId)
                ? RepoTeamCondition
                : string.Empty;

        public string UserTeamJoin { get; } =
            !string.IsNullOrEmpty(filter.UserTeamId)
                ? UserTeamJoins
                : string.Empty;

        public string UserTeamWhere { get; } =
            !string.IsNullOrEmpty(filter.UserTeamId)
                ? RepoUserCondition
                : string.Empty;
    }
}
