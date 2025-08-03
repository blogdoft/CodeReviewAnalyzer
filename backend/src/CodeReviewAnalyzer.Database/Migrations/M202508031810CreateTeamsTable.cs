using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031810, description: "Create table Teams")]
public class M202508031810CreateTeamsTable : Migration
{
    public override void Up()
    {
        Create.Table("Teams")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_teams")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("tenantId")
                .AsInt16()
                .NotNullable()
                .Indexed("idx_fk_teams_tenant")
                .WithColumnDescription("Fk to identify wich tenant this team belongs to.")
            .WithColumn("sharedKey")
                .AsGuid()
                .NotNullable()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("externalId")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("When exists, a external identifier.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("A descriptive name to identify this team.");

        Create.Index("idx_teams_shared_key")
            .OnTable("Teams")
            .OnColumn("sharedKey")
                .Unique();

        Create.ForeignKey("fk_teams_tenant")
            .FromTable("Teams")
            .ForeignColumn("tenantId")
            .ToTable("Tenants")
            .PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("Teams");
    }
}
