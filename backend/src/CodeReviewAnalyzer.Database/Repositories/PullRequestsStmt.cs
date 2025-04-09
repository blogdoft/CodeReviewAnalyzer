namespace CodeReviewAnalyzer.Database.Repositories;

internal static class PullRequestsStmt
{
    public const string InsertSql =
        """
            INSERT INTO "PULL_REQUEST" (
                "EXTERNAL_ID",
                "TITLE",
                "CREATED_BY_ID",
                "REPOSITORY_ID",
                "URL",
                "CREATION_DATE",
                "CLOSED_DATE",
                "FIRST_COMMENT_DATE",
                "LAST_APPROVAL_DATE",
                "REVISION_WAITING_TIME_MINUTES",
                "MERGE_WAITING_TIME_MINUTES",
                "FIRST_COMMENT_WAITING_TIME_MINUTES",
                "MERGE_MODE",
                "FILE_COUNT",
                "THREAD_COUNT"
            )
            VALUES (
                @ExternalId,
                @Title,
                (SELECT u."ID" FROM "USERS" u WHERE u."EXTERNAL_IDENTIFIER" = @UserKey),
                (SELECT r.id FROM "REPOSITORIES" r WHERE r.external_id = @RepositoryId),
                @Url,
                @CreationDate,
                @ClosedDate,
                @FirstCommentDate,
                @LastApprovalDate,
                @RevisionWaitingTimeMinutes,
                @MergeWaitingTimeMinutes,
                @FirstCommentWaitingTimeMinutes,
                @MergeMode,
                @FileCount,
                @ThreadCount
            )
            ON CONFLICT ("EXTERNAL_ID") DO UPDATE
            SET
                "TITLE" = EXCLUDED."TITLE",
                "CREATED_BY_ID" = EXCLUDED."CREATED_BY_ID",
                "REPOSITORY_ID" = EXCLUDED."REPOSITORY_ID",
                "URL" = EXCLUDED."URL",
                "CREATION_DATE" = EXCLUDED."CREATION_DATE",
                "CLOSED_DATE" = EXCLUDED."CLOSED_DATE",
                "FIRST_COMMENT_DATE" = EXCLUDED."FIRST_COMMENT_DATE",
                "LAST_APPROVAL_DATE" = EXCLUDED."LAST_APPROVAL_DATE",
                "REVISION_WAITING_TIME_MINUTES" = EXCLUDED."REVISION_WAITING_TIME_MINUTES",
                "MERGE_WAITING_TIME_MINUTES" = EXCLUDED."MERGE_WAITING_TIME_MINUTES",
                "FIRST_COMMENT_WAITING_TIME_MINUTES" = EXCLUDED."FIRST_COMMENT_WAITING_TIME_MINUTES",
                "MERGE_MODE" = EXCLUDED."MERGE_MODE",
                "FILE_COUNT" = EXCLUDED."FILE_COUNT",
                "THREAD_COUNT" = EXCLUDED."THREAD_COUNT"
            RETURNING "ID";

        """;

    public const string InsertComments =
        """
            INSERT INTO "PULL_REQUEST_COMMENTS" (
                "PULL_REQUEST_ID",
                "USER_ID",
                "COMMENT_INDEX",
                "THREAD_ID",
                "COMMENT_DATE",
                "COMMENT",
                "RESOLVED_DATE"
            )
            VALUES (
                @PullRequestId,
                (SELECT u."ID" FROM "USERS" u WHERE u."EXTERNAL_IDENTIFIER" = @UserId),
                @CommentIndex,
                @ThreadId,
                @CommentDate,
                @Comment,
                @ResolvedDate
            )
            ON CONFLICT ("PULL_REQUEST_ID", "THREAD_ID", "COMMENT_INDEX") DO UPDATE
            SET
                "USER_ID" = EXCLUDED."USER_ID",
                "COMMENT_INDEX" = EXCLUDED."COMMENT_INDEX",
                "THREAD_ID" = EXCLUDED."THREAD_ID",
                "COMMENT_DATE" = EXCLUDED."COMMENT_DATE",
                "COMMENT" = EXCLUDED."COMMENT",
                "RESOLVED_DATE" = EXCLUDED."RESOLVED_DATE";

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
                , @Vote)
            ON CONFLICT ("PULL_REQUEST_ID", "USER_ID") DO UPDATE
            SET "VOTE" = @Vote;

        """;

    public const string InsertWorkItemRelations =
        """
            INSERT INTO public."PULL_REQUEST_WORKITEMS" (
                  pull_request_id
                , external_id
                , workitem_id
            ) values (
                  @PullRequestId
                , @ExternalId
                , (select w.id from "WORKITEMS" w where w.external_id = @ExternalId)
            )
            ON CONFLICT (pull_request_id, external_id)
            DO UPDATE  SET 
                  pull_request_id = @PullRequestId
                , external_id = @ExternalId
                , workitem_id = (select w.id from "WORKITEMS" w where w.external_id = @ExternalId)

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
