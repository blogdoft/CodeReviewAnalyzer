using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508082004, description: "Create table to store Pull Request.")]
public class M202508082004CreateTablePullRequest : Migration
{
    public override void Up()
    {
        CreatePullRequestsTable();
        CreateTablePullRequestReviewers();
        CreateTablePullRequestComments();
    }

    public override void Down()
    {
        Delete.Table("pull_request_comments");
        Delete.Table("pull_requests_reviewer");
        Delete.Table("pull_requests");
    }

    private void CreatePullRequestsTable()
    {
        Create.Table("pull_requests")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_pull_requests")
                .NotNullable()
                .WithColumnDescription("Primary Key.")
            .WithColumn("tenant_id")
                .AsInt16()
                .NotNullable()
                    .ForeignKey("fk_pull_requests__tenant_id", "tenants", "id")
                .Indexed("idx_fk_pull_requests__tenant_id")
                .WithColumnDescription("FK for tenant.")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .Unique("uk_pull_requests__shared_key")
                .Indexed("idx_uk_pull_requests__shared_key")
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("title")
                .AsString(516)
                .NotNullable()
                .WithColumnDescription("A Pull Request short description.")
            .WithColumn("person_id_created_by")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_requests__person_id_created_by", "people", "id")
                .Indexed("idx_fk_pull_requests__person_id_created_by")
                .WithColumnDescription("Person who created the pull request.")
            .WithColumn("repository_id")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_requests__repository_id", "repositories", "id")
                .Indexed("idx_fk_pull_requests__repository_id")
                .WithColumnDescription("Repository related to this pull request.")
            .WithColumn("url")
                .AsString(1024)
                .NotNullable()
                .WithColumnDescription("Pull request URL")
            .WithColumn("creation_date")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("Pull request creation date")
            .WithColumn("closed_date")
                .AsDateTime()
                .NotNullable()
                .Indexed("idx_sh_pull_requests__closed_date")
                .WithColumnDescription("Pull request closed date")
            .WithColumn("first_comment_date")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("First comment date")
            .WithColumn("last_approval_date")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("When this PR was approved for the last time.")
            .WithColumn("first_comment_waiting_time_minutes")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Waiting time, in minutes, until first comment is made.")
                .WithDefaultValue(0)
            .WithColumn("revision_waiting_time_minutes")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Waiting time, in minutes, until revision is completed.")
            .WithColumn("merge_waiting_time_minutes")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Waiting time, in minutes, until merge is completed.")
            .WithColumn("merge_mode")
                .AsString(100)
                .NotNullable()
                .WithColumnDescription("Merge mode")
            .WithColumn("file_count")
                .AsInt64()
                .NotNullable()
                .WithColumnDescription("Number of files in the pull request")
                .WithDefaultValue(0)
            .WithColumn("thread_count")
                .AsInt16()
                .NotNullable()
                .WithDefaultValue(0)
                .WithColumnDescription("Number of threads in the pull request");

        Execute.Sql(@"
            CREATE INDEX idx_sh_pull_requests_closed_date_covering 
            ON public.""pull_requests""(""closed_date"")
            INCLUDE (""first_comment_waiting_time_minutes"", 
                     ""revision_waiting_time_minutes"", 
                     ""merge_waiting_time_minutes"", 
                     ""file_count"", 
                     ""thread_count"");
        ");

        Create.Index("idx_uk_pull_requests__external_id")
            .OnTable("pull_requests")
                .OnColumn("external_id")
                    .Unique()
                .OnColumn("tenant_id")
                    .Unique();
    }

    private void CreateTablePullRequestReviewers()
    {
        Create.Table("pull_requests_reviewer")
            .WithColumn("pull_request_id")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_requests_reviewer__pull_request_id", "pull_requests", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_pull_requests_reviewer__pull_request_id")
                .WithColumnDescription("Pull request identifier")
            .WithColumn("person_id")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_requests_reviewer__person_id", "people", "id")
                .Indexed("idx_fk_pull_requests_reviewer__person_id")
                .WithColumnDescription("User identifier")
            .WithColumn("vote")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Reviewer vote");
    }

    private void CreateTablePullRequestComments()
    {
        Create.Table("pull_request_comments")
            .WithColumn("id")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_pull_request_comments")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("pull_request_id")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_request_comments__pull_request_id", "pull_requests", "id")
                .Indexed("idx_fk_pull_request_comments__pull_request_id")
                .WithColumnDescription("Pull request identifier")
            .WithColumn("comment_index")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription(
                    """
                    Comment order inside a thread.
                    1 will be the first comment, usually made by the thread author.
                    
                    """)
            .WithColumn("thread_id")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription(
                    """
                    Group comments in different commit threads.
                    
                    """)
            .WithColumn("person_id")
                .AsInt32()
                .NotNullable()
                .ForeignKey("fk_pull_request_comments__person_id", "people", "id")
                .Indexed("idx_fk_pull_request_comments__person_id")
                .WithColumnDescription("User identifier")
            .WithColumn("comment_date")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("Comment date")
            .WithColumn("comment")
                .AsString(int.MaxValue)
                .NotNullable()
                .WithColumnDescription("Comment")
            .WithColumn("resolved_date")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("Resolved date");

        Create.Index("idx_uk_pull_request_comments")
            .OnTable("pull_request_comments")
            .OnColumn("pull_request_id")
                .Unique()
            .OnColumn("thread_id")
                .Unique()
            .OnColumn("comment_index")
                .Unique();
    }
}
