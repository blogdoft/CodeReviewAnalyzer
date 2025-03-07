namespace CodeReviewAnalyzer.Database.Repositories;

internal static class PullRequestsStmt
{
    public const string InsertSql =
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

    public const string InsertComments =
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

    public const string InsertReviewer =
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

    public const string SelectStats =
        """
            select pr."EXTERNAL_ID" as "ExternalId"
                , pr."TITLE"
                , pr."CREATION_DATE" as "CreatedAt"
                , pr."CLOSED_DATE" as "ClosedAt"
                , pr."FIRST_COMMENT_DATE" as "FirstCommentDate"
                , pr."FIRST_COMMENT_WAITING_TIME_MINUTES" as "FirstCommentWaitingMinutes"
                , pr."REVISION_WAITING_TIME_MINUTES" as "RevisionWaitingTimeMinutes"
                , pr."MERGE_WAITING_TIME_MINUTES" as "MergeWaitingTimeMinutes"
                , pr."MERGE_MODE" as "MergeMode"
                , pr."FILE_COUNT" as "FileCount"
                , pr."THREAD_COUNT" as "ThreadCount"
            from "PULL_REQUEST" pr 
            where pr."EXTERNAL_ID" = @ExternalId

        """;
}
