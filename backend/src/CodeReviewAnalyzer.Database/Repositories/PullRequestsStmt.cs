namespace CodeReviewAnalyzer.Database.Repositories;

internal static class PullRequestsStmt
{
    public const string InsertSql =
        """
            INSERT INTO public.pull_requests (
                  tenant_id
                , shared_key
                , external_id
                , title
                , person_id_created_by
                , repository_id
                , url
                , creation_date
                , closed_date
                , first_comment_date
                , last_approval_date
                , first_comment_waiting_time_minutes
                , revision_waiting_time_minutes
                , merge_waiting_time_minutes
                , merge_mode
                , file_count
                , thread_count
            ) VALUES (
                  @tenantId
                , @sharedKey
                , @externalId
                , @Title
                , (select p.id FROM people p where p.external_id = @personId)
                , (select r.id from repositories where r.external_id = @repositoriesId)
                , @url
                , @CreationDate
                , @ClosedDate
                , @FirstCommentDate
                , @LastApprovalDate
                , @FirstCommentWaitingTimeMinutes
                , @RevisionWaitingTimeMinutes
                , @MergeWaitingTimeMinutes
                , @MergeMode
                , @FileCount
                , @ThreadCount
            )
            ON CONFLICT (id)
            DO UPDATE SET 
                  tenant_id=@tenantId
                , shared_key=@sharedKey
                , external_id=@externalId
                , person_id_created_by=EXCLUDED.person_id_created_by
                , repository_id=EXCLUDED.repository_id
                , url=@url
                , creation_date=@creationDate
                , closed_date=@closedDate
                , first_comment_date=@firstCommentDate
                , last_approval_date=@lastApprovalDate
                , first_comment_waiting_time_minutes=@firstCommentWaitingTimeMinutes
                , revision_waiting_time_minutes=@revisionWaitingTimeMinutes
                , merge_waiting_time_minutes=@MergeWaitingTimeMinutes
                , merge_mode=@mergeMode
                , file_count=@fileCount
                , thread_count=@threadCount
            RETURNING id;        

        """;

    public const string InsertComments =
        """
            INTO public.pull_request_comments (
                  pull_request_id
                , comment_index
                , thread_id
                , person_id
                , comment_date
                , "comment"
                , resolved_date
            ) VALUES (
                  @pullRequestId,
                , @commentIndex
                , @threadId
                , (SELECT p.id FROM people p WHERE p.external_id = @personId)
                , @commentDate
                , @comment
                , @resolvedDate
            )
            ON CONFLICT (pull_request_id, thread_id, comment_index)
            DO UPDATE SET 
                  id=EXCLUDED.id
                , person_id=EXCLUDED.person_id
                , comment_date=EXCLUDED.comment_date
                , "comment"=EXCLUDED."comment"
                , resolved_date=EXCLUDED.resolved_date;                

        """;

    public const string InsertReviewer =
        """ 
            INSERT INTO public.pull_requests_reviewer (
                  pull_request_id
                , person_id
                , vote
            ) VALUES (
                  @pullRequestId
                , (select p.id from people p where p.external_id = @personId)
                , @vote
            )
            ON CONFLICT (pull_request_id, person_id)
            DO UPDATE SET vote=@vote;            

        """;

    public const string InsertWorkItemRelations =
        """
               

        """;

    public const string SelectStats =
        """
            SELECT pr.tenant_id as "tenantId"
                 , pr.shared_key as "sharedKey"
                 , pr.external_id as "externalKey"
                 , pr.creation_date as "CreatedAt"
                 , pr.closed_date as "ClosedAt"
                 , pr.first_comment_date as "FirstCommentDate"
                 , pr.first_comment_waiting_time_minutes as "FirstCommentWaitingMinutes"
                 , pr.revision_waiting_time_minutes as "RevisionWaitingTimeMinutes"
                 , pr.merge_waiting_time_minutes as "MergeWaitingTimeMinutes"
                 , pr.merge_mode as "MergeMode"
                 , pr.file_count as "FileCount"
                 , pr.thread_count as "ThreadCount"
            FROM public.pull_requests pr
            where pr.external_id = @externalId
           
        """;
}
