using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;

namespace CodeReviewAnalyzer.Database.Repositories;

public class PullRequestsRepository(IDatabaseFacade databaseFacade) : IPullRequests
{
    private const string InsertSql =
        """
            INSERT INTO "PULL_REQUEST" (
                  "EXTERNAL_ID"
                , "TITLE"
                , "CREATED_BY_ID"
                , "REPOSITORY_ID"
                , "URL"
                , "CREATION_DATE"
                , "CLOSED_DATE"
                , "FIRST_COMMENT_DATE"
                , "LAST_APPROVAL_DATE"
                , "REVISION_WAITING_TIME_MINUTES"
                , "MERGE_WAITING_TIME_MINUTES"
                , "FIRST_COMMENT_WAITING_TIME_MINUTES"
                , "MERGE_MODE"
                , "FILE_COUNT"
                , "THREAD_COUNT"
            ) VALUES(
                  @ExternalId
                , @Title
                , (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = @UserKey)
                , (select r.id from "REPOSITORIES" r where r.external_id = @RepositoryId)
                , @Url
                , @CreationDate
                , @ClosedDate
                , @FirstCommentDate
                , @LastApprovalDate
                , @RevisionWaitingTimeMinutes
                , @MergeWaitingTimeMinutes
                , @FirstCommentWaitingTimeMinutes
                , @MergeMode
                , @FileCount
                , @ThreadCount) returning "ID";

        """;

    private const string InsertComments =
        """
            INSERT INTO "PULL_REQUEST_COMMENTS" (
                  "PULL_REQUEST_ID"
                , "USER_ID"
                , "COMMENT_INDEX"
                , "THREAD_ID"
                , "COMMENT_DATE"
                , "COMMENT"
                , "RESOLVED_DATE"
            ) VALUES (
                  @PullRequestId
                , (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = @UserId)
                , @CommentIndex
                , @ThreadId
                , @CommentDate
                , @Comment
                , @ResolvedDate
            );

        """;

    private const string InsertReviewer =
        """ 
            INSERT INTO public."PULL_REQUEST_REVIEWER" (
                  "PULL_REQUEST_ID"
                , "USER_ID"
                , "VOTE"
            ) VALUES(
                  @PullRequestId
                , (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = @UserId)
                , @Vote);

        """;

    public async Task Add(PullRequest pullRequest)
    {
        var id = await databaseFacade.ExecuteScalarAsync<int>(InsertSql, new
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
            InsertComments,
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
            await databaseFacade.ExecuteAsync(InsertReviewer, new
            {
                PullRequestId = id,
                UserId = reviewer.User.Id,
                reviewer.Vote,
            });
        }
    }
}
