using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508082044, description: "Create table WorkItem.")]
public class M202508082044CreateTableWorkItem : Migration
{
    public override void Up()
    {
        CreateWorkItems();
        CreateWorkItemsPullRequestsRelation();
        CreateWorkItemSelfRelation();
    }

    public override void Down()
    {
        Delete.Table("work_item_self_relation");
        Delete.Table("work_items_pull_requests");
        Delete.Table("work_items");
    }

    private void CreateWorkItems()
    {
        Create.Table("work_items")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_work_items")
                .NotNullable()
                .WithColumnDescription("Primary Key.")
            .WithColumn("tenants_id")
                .AsInt16()
                .NotNullable()
                .ForeignKey("fk_work_items_tenants", "tenants", "id")
                .Indexed("idx_fk_work_items_tenants")
                .WithColumnDescription("FK for tenant.")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id.")
            .WithColumn("work_item_type")
                .AsString(255)
                .NotNullable()
                .Indexed("idx_sh_work_items_type")
                .WithColumnDescription("Describes the work item type. E.g.: Story, Defect, Bug and etc.")
            .WithColumn("group")
                .AsString(516)
                .Nullable()
                .Indexed("idx_sh_work_items_group")
                .WithColumnDescription("An strings thats groups the Work Item (a.k.a.: Area path on Azure DevOps).")
            .WithColumn("title")
                .AsString(1024)
                .NotNullable()
                .WithColumnDescription("The work item title.")
            .WithColumn("created_at")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("When work item was created.")
            .WithColumn("closed_at")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("When work item was closed.")
            .WithColumn("canceled")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(false)
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

        Create.Index("idx_uk_work_items_external_id")
            .OnTable("work_items")
                .OnColumn("external_id")
                    .Unique()
                .OnColumn("tenants_id")
                    .Unique();
    }

    private void CreateWorkItemsPullRequestsRelation()
    {
        Create.Table("work_items_pull_requests")
            .WithColumn("tenants_id")
                .AsInt16()
                .NotNullable()
                .ForeignKey("fk_work_items_pull_requests_tenants", "tenants", "id")
                .WithColumnDescription("FK for tenants")
            .WithColumn("work_items_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_work_items_pull_requests_work_item", "work_items", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumnDescription("Fk to work item.")
            .WithColumn("work_items_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late relation")
            .WithColumn("pull_requests_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_work_Items_pull_requests_pull_requests", "pull_requests", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumnDescription("Fk to pull request")
            .WithColumn("pull_requests_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late relation");

        Create.Index("idx_uk_wipr_work_items_external_id")
            .OnTable("work_items_pull_requests")
                .OnColumn("tenants_id")
                .Ascending()
                .OnColumn("work_items_external_id")
                .Unique();

        Create.Index("idx_uk_wipr_pull_requests_external_id")
            .OnTable("work_items_pull_requests")
                .OnColumn("tenants_id")
                .Ascending()
                .OnColumn("pull_requests_external_id")
                .Unique();
    }

    private void CreateWorkItemSelfRelation()
    {
        Create.Table("work_item_self_relation")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .NotNullable()
                .PrimaryKey("idx_pk_work_item_self_relation")
            .WithColumn("work_items_id_root")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_work_item_self_relation_wi_root", "work_items", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_work_item_self_relation_wi_root")
                .WithColumnDescription("The work item root")
            .WithColumn("work_items_id_related")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_work_item_self_relation_wi_related", "work_items", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_work_item_self_relation_wi_related")
                .WithColumnDescription("The related work item")
            .WithColumn("relation_type")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("The relation kind between work items");
    }
}
