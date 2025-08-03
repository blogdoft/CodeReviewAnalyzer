using FluentMigrator;
using FluentMigrator.Postgres;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031908, description: "Create relation between Team and People")]
public class M202508031908CreateRelationTeamsPerson : Migration
{
    public override void Up()
    {
        Create.Table("TeamPeople")
            .WithColumn("teamId")
                .AsInt64()
                .NotNullable()
                .WithColumnDescription("FK to Team table.")
            .WithColumn("teamExternalId")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("personId")
                .AsInt64()
                .NotNullable()
                .WithColumnDescription("Fk to Person table.")
            .WithColumn("personExternalId")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("tenantId")
                .AsInt16()
                .NotNullable()
                .WithColumnDescription("FK for tenant. Necessary for late merge.");

        Create.ForeignKey("fk_teamPeople_team")
            .FromTable("TeamPeople")
            .ForeignColumn("teamId")
            .ToTable("Teams")
            .PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.SetNull);

        Create.ForeignKey("fk_teamPeople_people")
            .FromTable("TeamPeople")
            .ForeignColumn("personId")
            .ToTable("People")
            .PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.SetNull);

        Create.Index("idx_sh_teamPeople_teamExternalId")
            .OnTable("TeamPeople")
            .OnColumn("teamExternalId")
            .Ascending()
            .OnColumn("tenantId")
            .Unique()
            .NullsNotDistinct();

        Create.Index("idx_sh_teamPeople_personExternalId")
            .OnTable("TeamPeople")
            .OnColumn("personExternalId")
            .Unique()
            .NullsNotDistinct()
            .OnColumn("tenantId")
            .Unique()
            .NullsNotDistinct();
    }

    public override void Down()
    {
        Delete.Table("TeamPeople");
    }
}
