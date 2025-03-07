using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;

namespace CodeReviewAnalyzer.Database.Repositories;

internal class PullRequestsRepository(IDatabaseFacade databaseFacade) : IPullRequests
{
    public async Task Add(PullRequest pullRequest)
    {
        var id = await databaseFacade.ExecuteScalarAsync<int>(
            PullRequestsStmt.InsertSql,
            new
            {
                ExternalId = pullRequest.Id.ToString(),
                pullRequest.Title,
                RepositoryId = pullRequest.Repository.ExternalId,
                CreationDate = pullRequest.CreationDate.ToLocalTime(),
                ClosedDate = pullRequest.ClosedDate.ToLocalTime(),
                pullRequest.Url,
                UserKey = pullRequest.CreatedBy.Id,
                RevisionWaitingTimeMinutes = pullRequest.RevisionWaitingTime.TotalMinutes,
                MergeWaitingTimeMinutes = pullRequest.MergeWaitingTime.TotalMinutes,
                FirstCommentWaitingTimeMinutes = pullRequest.FirstCommentWaitingTime.TotalMinutes,
                FirstCommentDate = pullRequest.FirstCommentDate.ToLocalTime(),
                LastApprovalDate = pullRequest.LastApprovalDate.ToLocalTime(),
                pullRequest.MergeMode,
                pullRequest.FileCount,
                pullRequest.ThreadCount,
            });
        foreach (var comment in pullRequest.Comments)
        {
            await databaseFacade.ExecuteAsync(
            PullRequestsStmt.InsertComments,
            new
            {
                PullRequestId = id,
                UserId = comment.CommentedBy.Id,
                comment.CommentIndex,
                comment.ThreadId,
                CommentDate = comment.CommentDate.ToLocalTime(),
                comment.Comment,
                ResolvedDate = comment.ResolvedDate.ToLocalTime(),
            });
        }

        foreach (var reviewer in pullRequest.Reviewers)
        {
            await databaseFacade.ExecuteAsync(PullRequestsStmt.InsertReviewer, new
            {
                PullRequestId = id,
                UserId = reviewer.User.Id,
                reviewer.Vote,
            });
        }
    }

    public async Task<PullRequestStats?> GetStatsFromAsync(string externalId)
    {
        var stats = await databaseFacade.QuerySingleOrDefaultAsync<PullRequestStats>(
            PullRequestsStmt.SelectStats,
            new { externalId });

        return stats;
    }
}
