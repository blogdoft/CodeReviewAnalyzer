namespace CodeReviewAnalyzer.Database.Repositories;

internal static class ReportStmt
{
    internal const string MeanTimeToReview =
       """
            select avg(pr.first_comment_waiting_time_minutes) as PeriodInMinutes
                , date_trunc('month', pr.closed_date) as ReferenceDate
            from public.pull_requests pr 
                join people p on p.id = pr.created_by_people_id 
                {0}                
            where pr.closed_date between @from and @to
              -- FirstCommentDate is equal to CreationDate when a pr has no comment.
              and pr.first_comment_date <> pr.creation_date 
              {1}            
            group by 2
            order by 2 asc

        """;

    internal const string MeanTimeToMerge =
        """
            -- Mean time to merge
            select avg(pr.merge_waiting_time_minutes) as PeriodInMinutes
                 , date_trunc('month', pr.closed_date) as ReferenceDate
            from pull_requests pr
                join people p on p.id = pr.created_by_people_id 
                {0}
            where pr.closed_date between @from and @to
            {1}
            group by 2
            order by 2 asc

        """;
    // aqui
    internal const string MeanTimeToApprove =
        """
            select (extract(epoch from  avg(pr."last_approval_date" - pr."creation_date")) / 60 )::int as PeriodInMinutes
                 , date_trunc('month', pr."closed_date" ) as ReferenceDate
            from "pull_requests" pr 
                join "people" p on p."id" = pr."created_by_people_id"
                {0}
            where pr."closed_date" between @From and @To
                {1}
            group by 2
            order by 2 asc

        """;

    internal const string PullRequestCount =
        """
            -- Pull Request count
            select count(1) as PeriodInMinutes
                , date_trunc('month', pr."closed_date" ) as ReferenceDate
            from "pull_requests" pr  
                join "people" p on p."id" = pr."created_by_people_id" 
                {0}
            where pr."closed_date" between @From and @To
            {1}
            group by 2
            order by 2 asc
        
        """;

    internal const string ApprovedOnFirstAttempt =
        """
            select count(1) as PeriodInMinutes
                , date_trunc('month', pr."closed_date" ) as ReferenceDate
            from "pull_requests" pr  
                join "people" p on p."id" = pr."created_by_people_id" 
                {0}
            where pr."closed_date" between @From and @To
              and pr."thread_count" = 0
              {1}
            group by 2
            order by 2 asc


        """;

    internal const string PullRequestStats =
        """
            -- Pull Request counters
            select avg(pr."file_count") as "MeanFileCount"
                , max(pr."file_count") as "MaxFileCount"
                , min(pr."file_count") as "MinFileCount"
                , count(pr.id) as "PrCount"
                , date_trunc('month', pr."closed_date" ) as "ReferenceDate"
            from "pull_requests" pr
                join "people" p on p."id" = pr."created_by_people_id" 
                {0}
            where pr."closed_date" between @From and @To
            {1}
            group by "ReferenceDate"
            order by "ReferenceDate" asc

        """;

    internal const string UserReviewerDensitySql =
        """
            select p."id" as "UserId"
                 , p."name" as "UserName"
                 , date_trunc('month', pr."closed_date" ) as "ReferenceDate"
                 , count(1) as "CommentCount"
            from "pull_requests" pr
                join "pull_requests_reviewer" prr on prr."pull_requests_id" = pr.id and prr."vote" > 0 
                join "people" p on p."id" = prr."people_id"
                {0}
            where pr."closed_date" between :From and :To
                  {1}  
            group by "UserId"
                   , "UserName"
                   , "ReferenceDate"
        
        """;

    internal const string PullRequestOutLiers =
        """
            WITH filtered AS (
            SELECT pr.*
            FROM public."pull_requests" pr
                join "people" p on p."id" = pr."created_by_people_id"
                {0}
            WHERE pr."closed_date" BETWEEN :from AND :to
                {1}
            ),
            fc_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "first_comment_waiting_time_minutes") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "first_comment_waiting_time_minutes") AS q3
            FROM filtered
            ),
            rev_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "revision_waiting_time_minutes") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "revision_waiting_time_minutes") AS q3
            FROM filtered
            ),
            merge_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "merge_waiting_time_minutes") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "merge_waiting_time_minutes") AS q3
            FROM filtered
            ),
            file_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "file_count") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "file_count") AS q3
            FROM filtered
            ),
            thread_stats AS (
            SELECT
                percentile_cont(0.25) WITHIN GROUP (ORDER BY "thread_count") AS q1,
                percentile_cont(0.75) WITHIN GROUP (ORDER BY "thread_count") AS q3
            FROM filtered
            )

            -- Outliers para first_comment_waiting_time_minutes
            SELECT 'First comment waiting time (h)' AS "OutlierField"
                , f."first_comment_waiting_time_minutes" / 60 as "OutlierValue"
                , f.url
                , f.external_id as "ExternalId"
            FROM filtered f, fc_stats s
            WHERE f."first_comment_waiting_time_minutes" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."first_comment_waiting_time_minutes" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para revision_waiting_time_minutes
            SELECT 'Revision waiting time (h)' AS "OutlierField"
                , f."revision_waiting_time_minutes" / 60 as "OutlierValue"
                , f.url
                , f.external_id as "ExternalId"
            FROM filtered f, rev_stats s
            WHERE f."revision_waiting_time_minutes" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."revision_waiting_time_minutes" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para merge_waiting_time_minutes
            SELECT 'Merge waiting time (h)' AS "OutlierField",
                f."merge_waiting_time_minutes" / 60 as "OutlierValue"
                , f.url
                , f.external_id as "ExternalId"
            FROM filtered f, merge_stats s
            WHERE f."merge_waiting_time_minutes" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."merge_waiting_time_minutes" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para file_count
            SELECT 'File count' AS "OutlierField"
                , f."file_count" as "OutlierValue"
                , f.url
                , f.external_id as "ExternalId"
            FROM filtered f, file_stats s
            WHERE f."file_count" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."file_count" > s.q3 + 1.5 * (s.q3 - s.q1)

            UNION ALL

            -- Outliers para thread_count
            SELECT 'Thread Count' AS "OutlierField"
                , f."thread_count" as "OutlierValue"
                , f.url
                , f.external_id as "ExternalId"
            FROM filtered f, thread_stats s
            WHERE f."thread_count" < s.q1 - 1.5 * (s.q3 - s.q1)
            OR f."thread_count" > s.q3 + 1.5 * (s.q3 - s.q1);


        """;
}
