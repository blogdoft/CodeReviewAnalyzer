using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202501311748, description: "Create Pull Request structure")]
public class M202501311748CreatePullRequestTable : Migration
{
    public override void Up()
    {
        Create.Table("PULL_REQUEST")
            .WithColumn("ID")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_pull_request")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("EXTERNAL_ID")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("External identifier provided by source")
            .WithColumn("TITLE")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Pull request title")
            .WithColumn("CREATED_BY_ID")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_request_user", "USERS", "ID")
                .WithColumnDescription("User who created the pull request")
            .WithColumn("REPOSITORY_ID")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Repository related to this pull request.")
                .ForeignKey("REPOSITORIES", "id")
            .WithColumn("URL")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Pull request URL")
            .WithColumn("CREATION_DATE")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("Pull request creation date")
            .WithColumn("CLOSED_DATE")
                .AsDateTime()
                .NotNullable()
                .Indexed("idx_pull_request_closed_date")
                .WithColumnDescription("Pull request closed date")
            .WithColumn("FIRST_COMMENT_DATE")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("First comment date")
            .WithColumn("LAST_APPROVAL_DATE")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("When this PR was approved for the last time.")
            .WithColumn("FIRST_COMMENT_WAITING_TIME_MINUTES")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Waiting time, in minutes, until first comment is made.")
                .WithDefaultValue(0)
            .WithColumn("REVISION_WAITING_TIME_MINUTES")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Waiting time, in minutes, until revision is completed.")
            .WithColumn("MERGE_WAITING_TIME_MINUTES")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Waiting time, in minutes, until merge is completed.")
            .WithColumn("MERGE_MODE")
                .AsString(100)
                .NotNullable()
                .WithColumnDescription("Merge mode")
            .WithColumn("FILE_COUNT")
                .AsInt64()
                .NotNullable()
                .WithColumnDescription("Number of files in the pull request")
                .WithDefaultValue(0)
            .WithColumn("THREAD_COUNT")
                .AsInt16()
                .NotNullable()
                .WithDefaultValue(0)
                .WithColumnDescription("Number of threads in the pull request");

        Execute.Sql(@"
            CREATE INDEX idx_pull_request_closed_date_covering 
            ON public.""PULL_REQUEST""(""CLOSED_DATE"")
            INCLUDE (""FIRST_COMMENT_WAITING_TIME_MINUTES"", 
                     ""REVISION_WAITING_TIME_MINUTES"", 
                     ""MERGE_WAITING_TIME_MINUTES"", 
                     ""FILE_COUNT"", 
                     ""THREAD_COUNT"");
        ");

        Create.Index("idx_uk_pull_request_external_id")
            .OnTable("PULL_REQUEST")
            .OnColumn("EXTERNAL_ID")
            .Unique();

        Create.Table("PULL_REQUEST_REVIEWER")
            .WithColumn("PULL_REQUEST_ID")
                .AsInt32()
                .NotNullable()
                .ForeignKey("idx_fk_pull_request_reviewer_pull_request", "PULL_REQUEST", "ID")
                .WithColumnDescription("Pull request identifier")
            .WithColumn("USER_ID")
                .AsInt32()
                .NotNullable()
                .ForeignKey("idx_fk_pull_request_reviewer_user", "USERS", "ID")
                .WithColumnDescription("User identifier")
            .WithColumn("VOTE")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Reviewer vote");
        Create.Index("IDX_FK_PULL_REQUEST_USER")
            .OnTable("PULL_REQUEST")
            .OnColumn("CREATED_BY_ID")
            .Ascending();

        Create.Index("idx_uk_pull_request_reviewer")
            .OnTable("PULL_REQUEST_REVIEWER")
            .OnColumn("PULL_REQUEST_ID")
                .Unique()
            .OnColumn("USER_ID")
                .Unique();

        Create.Table("PULL_REQUEST_COMMENTS")
            .WithColumn("ID")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_pull_request_comments")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("PULL_REQUEST_ID")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_request_comments_pull_request", "PULL_REQUEST", "ID")
                .Indexed("idx_fk_pull_request_comments_pull_request")
                .WithColumnDescription("Pull request identifier")
            .WithColumn("COMMENT_INDEX")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription(
                    """
                    Comment order inside a thread.
                    1 will be the first comment, usually made by the thread author.
                    
                    """)
            .WithColumn("THREAD_ID")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription(
                    """
                    Group comments in different commit threads.
                    
                    """)
            .WithColumn("USER_ID")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_request_comments_user", "USERS", "ID")
                .Indexed("idx_fk_pull_request_comments_user")
                .WithColumnDescription("User identifier")
            .WithColumn("COMMENT_DATE")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("Comment date")
            .WithColumn("COMMENT")
                .AsString(int.MaxValue)
                .NotNullable()
                .WithColumnDescription("Comment")
            .WithColumn("RESOLVED_DATE")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("Resolved date");

        Create.Index("idx_uk_pull_requests_comment")
            .OnTable("PULL_REQUEST_COMMENTS")
            .OnColumn("PULL_REQUEST_ID")
                .Unique()
            .OnColumn("THREAD_ID")
                .Unique()
            .OnColumn("COMMENT_INDEX")
                .Unique();
    }

    public override void Down()
    {
        Delete.Index("idx_pull_request_closed_date_covering").OnTable("PULL_REQUEST");
        Delete.Table("PULL_REQUEST_COMMENTS");
        Delete.Table("PULL_REQUEST_REVIEWER");
        Delete.Table("PULL_REQUEST");
    }
}
