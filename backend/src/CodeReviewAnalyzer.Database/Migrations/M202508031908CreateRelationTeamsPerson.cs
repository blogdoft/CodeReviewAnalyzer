using FluentMigrator;
using FluentMigrator.Postgres;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031908, description: "Create relation between Team and People")]
public class M202508031908CreateRelationteamsPerson : Migration
{
    public override void Up()
    {
        Create.Table("teams_people")
            .WithColumn("tenants_id")
                .AsInt16()
                .NotNullable()
                .WithColumnDescription("FK for tenant. Necessary for late merge.")
            .WithColumn("teams_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_people_teams", "teams", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_teams_people_teams")
                .WithColumnDescription("FK to Team table.")
            .WithColumn("teams_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("people_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_people_people_id", "people", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumnDescription("Fk to Person table.")
            .WithColumn("people_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("role")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Which role this person has on team.")
            .WithColumn("joined_at_utc")
                .AsDateTime()
                .NotNullable()
                .WithColumnDescription("");

        Create.Index("idx_sh_teams_people_teams_external_id")
             .OnTable("teams_people")
                 .OnColumn("teams_external_id")
                     .Ascending()
                 .OnColumn("tenants_id")
                     .Unique()
                     .NullsNotDistinct();

        Create.Index("idx_sh_teams_people_person_external_id")
            .OnTable("teams_people")
                .OnColumn("people_external_id")
                    .Unique()
                    .NullsNotDistinct()
                .OnColumn("tenants_id")
                    .Unique()
                    .NullsNotDistinct();
    }

    public override void Down() =>
        Delete.Table("teams_people");
}
