namespace CodeReviewAnalyzer.Database.Repositories;

internal static class ReportStmt
{
    internal const string MeanTimeToReview =
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

    internal const string MeanTimeToMerge =
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

    internal const string MeanTimeToApprove =
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

    internal const string PullRequestCount =
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

    internal const string ApprovedOnFirstAttempt =
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

    internal const string PullRequestStats =
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

    internal const string UserReviewerDensitySql =
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

    internal const string PullRequestOutLiers =
        """
            WITH filtered AS (
            SELECT pr.*
            FROM public."PULL_REQUEST" pr
                join "USERS" u on u."ID" = pr."CREATED_BY_ID" and u."ACTIVE"
                {0}
            WHERE pr."CLOSED_DATE" BETWEEN :from AND :to
                {1}
            ),
            fc_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "FIRST_COMMENT_WAITING_TIME_MINUTES") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "FIRST_COMMENT_WAITING_TIME_MINUTES") AS q3
            FROM filtered
            ),
            rev_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "REVISION_WAITING_TIME_MINUTES") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "REVISION_WAITING_TIME_MINUTES") AS q3
            FROM filtered
            ),
            merge_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "MERGE_WAITING_TIME_MINUTES") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "MERGE_WAITING_TIME_MINUTES") AS q3
            FROM filtered
            ),
            file_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "FILE_COUNT") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "FILE_COUNT") AS q3
            FROM filtered
            ),
            thread_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "THREAD_COUNT") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "THREAD_COUNT") AS q3
            FROM filtered
            )

            -- Outliers para FIRST_COMMENT_WAITING_TIME_MINUTES
            SELECT 'First comment waiting time (h)' AS "OutlierField"
                , f."FIRST_COMMENT_WAITING_TIME_MINUTES" / 60 as "OutlierValue"
                , f."URL"
            FROM filtered f, fc_stats s
            WHERE f."FIRST_COMMENT_WAITING_TIME_MINUTES" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."FIRST_COMMENT_WAITING_TIME_MINUTES" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para REVISION_WAITING_TIME_MINUTES
            SELECT 'Revision waiting time (h)' AS "OutlierField"
                , f."REVISION_WAITING_TIME_MINUTES" / 60 as "OutlierValue"
                , f."URL"
            FROM filtered f, rev_stats s
            WHERE f."REVISION_WAITING_TIME_MINUTES" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."REVISION_WAITING_TIME_MINUTES" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para MERGE_WAITING_TIME_MINUTES
            SELECT 'Merge waiting time (h)' AS "OutlierField",
                f."MERGE_WAITING_TIME_MINUTES" / 60 as "OutlierValue"
                , f."URL"
            FROM filtered f, merge_stats s
            WHERE f."MERGE_WAITING_TIME_MINUTES" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."MERGE_WAITING_TIME_MINUTES" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para FILE_COUNT
            SELECT 'File count' AS "OutlierField"
                , f."FILE_COUNT" as "OutlierValue"
                , f."URL"
            FROM filtered f, file_stats s
            WHERE f."FILE_COUNT" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."FILE_COUNT" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para THREAD_COUNT
            SELECT 'Thread Count' AS "OutlierField"
                , f."THREAD_COUNT" as "OutlierValue"
                , f."URL"
            FROM filtered f, thread_stats s
            WHERE f."THREAD_COUNT" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."THREAD_COUNT" > s.q3 + 1.5 * (s.q3 - s.q1);


        """;
}
