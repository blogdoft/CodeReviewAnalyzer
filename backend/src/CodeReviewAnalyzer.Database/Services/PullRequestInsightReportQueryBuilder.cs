using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Database.Repositories;

namespace CodeReviewAnalyzer.Database.Services;

public static class PullRequestInsightReportQueryBuilder
{
    public static string BuildPullRequestSql(ReportFilter filter)
    {
        var sqlFilter = new RepoSqlFilter(filter);

        var meanTimeToReview = Merge(ReportStmt.MeanTimeToReview, sqlFilter);
        var meanTimeToApprove = Merge(ReportStmt.MeanTimeToApprove, sqlFilter);
        var meanTimeToMerge = Merge(ReportStmt.MeanTimeToMerge, sqlFilter);
        var pullRequestCount = Merge(ReportStmt.PullRequestCount, sqlFilter);
        var approvedOnFirstAttempt = Merge(ReportStmt.ApprovedOnFirstAttempt, sqlFilter);
        var pullRequestStats = Merge(ReportStmt.PullRequestStats, sqlFilter);

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
        return Merge(ReportStmt.UserReviewerDensitySql, sqlFilter);
    }

    public static string BuildOutlier(ReportFilter filter)
    {
        var sqlFilter = new RepoSqlFilter(filter);
        return Merge(ReportStmt.PullRequestOutLiers, sqlFilter);
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
            join "TEAMS" repo_team on repo_team.id = tr.teams_id
            
        """;

        private const string UserTeamJoins =
            """
            join "TEAM_USER" tu on tu.user_id = u."ID"
            join "TEAMS" user_team on user_team.id = tu.teams_id

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
            filter.RepoTeamId is not null && filter.RepoTeamId != Guid.Empty
                ? RepoTeamJoins
                : string.Empty;

        public string RepoTeamWhere { get; } =
            filter.RepoTeamId is not null && filter.RepoTeamId != Guid.Empty
                ? RepoTeamCondition
                : string.Empty;

        public string UserTeamJoin { get; } =
            filter.UserTeamId is not null && filter.UserTeamId != Guid.Empty
                ? UserTeamJoins
                : string.Empty;

        public string UserTeamWhere { get; } =
            filter.UserTeamId is not null && filter.UserTeamId != Guid.Empty
                ? RepoUserCondition
                : string.Empty;
    }
}
