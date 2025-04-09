namespace CodeReviewAnalyzer.Database.Repositories;

public static class PullRequestWorkitemsStmt
{
    public const string SyncByExternalId =
        """
            update public."PULL_REQUEST_WORKITEMS" set
                workitem_id = @WorkItemId
            where external_id = @ExternalId

        """;
}
