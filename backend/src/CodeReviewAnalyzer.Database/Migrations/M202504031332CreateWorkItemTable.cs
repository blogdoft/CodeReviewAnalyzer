using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202504031332, description: "Create WorkItem table")]
public class M202504031332CreateWorkItemTable : Migration
{
    public override void Up()
    {
        Create.Table("WORKITEMS")
            .WithColumn("id")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_workitems")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("external_id")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Work item id as into source tool.")
            .WithColumn("type")
                .AsString(128)
                .NotNullable()
                .WithColumnDescription("Work item type: User story, Technical Feature, Bug, etc.")
            .WithColumn("area_path")
                .AsString(516)
                .Nullable()
                .WithColumnDescription("Same as Azure AreaPath")
            .WithColumn("project")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Project related to this work item.")
            .WithColumn("title")
                .AsString(2000)
                .Nullable()
                .WithColumnDescription("Work item title.")
            .WithColumn("created_at")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("Work item creation date.")
            .WithColumn("closed_at")
                .AsDateTime()
                .Nullable()
                .WithColumnDescription("Work item closed date.");

        Create.Table("WORKITEMS_RELATION")
            .WithColumn("left_workitem_id")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Left work item id.")
            .WithColumn("right_workitem_id")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Right work item id.");

        Create.Table("PULL_REQUEST_WORKITEMS")
            .WithColumn("pull_request_id")
                .AsInt32()
                .NotNullable()
                .ForeignKey("PULL_REQUEST", "ID")
                .WithColumnDescription("Pull request id.")
            .WithColumn("external_id")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Work item id as into source tool. Use this for further link.")
            .WithColumn("workitem_id")
                .AsInt32()
                .Nullable()
                .ForeignKey("WORKITEMS", "id")
                .WithColumnDescription("Work item id.");

        Create.Index("idx_workitems_unique")
            .OnTable("WORKITEMS")
            .OnColumn("external_id")
                .Unique()
            .OnColumn("project")
                .Unique();

        Create.PrimaryKey("pk_workitems_relation")
            .OnTable("WORKITEMS_RELATION")
            .Columns("left_workitem_id", "right_workitem_id");

        Create.ForeignKey("fk_workitems_relation_left")
            .FromTable("WORKITEMS_RELATION")
            .ForeignColumn("left_workitem_id")
            .ToTable("WORKITEMS")
            .PrimaryColumn("id");
        Create.ForeignKey("fk_workitems_relation_right")
            .FromTable("WORKITEMS_RELATION")
            .ForeignColumn("right_workitem_id")
            .ToTable("WORKITEMS")
            .PrimaryColumn("id");

        Create.Index("idx_uk_pull_request_workitems_id_external_id")
            .OnTable("PULL_REQUEST_WORKITEMS")
            .OnColumn("pull_request_id")
                .Unique()
            .OnColumn("external_id")
                .Unique();
    }

    public override void Down()
    {
        Delete.Table("PULL_REQUEST_WORKITEMS");
        Delete.Table("WORKITEMS_RELATION");
        Delete.Table("WORKITEMS");
    }
}
