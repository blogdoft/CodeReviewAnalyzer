using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031908, description: "Create relation between Team and People")]
public class M202508031908CreateRelationteamsPerson : Migration
{
    public override void Up()
    {
        Create.Table("teams_people")
            .WithColumn("tenant_id")
                .AsInt16()
                .NotNullable()
                .WithColumnDescription("FK for tenant. Necessary for late merge.")
            .WithColumn("team_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_people__team_id", "teams", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_teams_people__team_id")
                .WithColumnDescription("FK to Team table.")
            .WithColumn("team_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("person_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_people__person_id", "people", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_teams_people__person_id")
                .WithColumnDescription("Fk to Person table.")
            .WithColumn("person_external_id")
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
                .WithColumnDescription(string.Empty);

        Create.Index("idx_sh_teams_people__team_external_id")
             .OnTable("teams_people")
                 .OnColumn("team_external_id")
                     .Ascending()
                 .OnColumn("tenant_id");

        Create.Index("idx_sh_teams_people__person_external_id")
            .OnTable("teams_people")
                .OnColumn("person_external_id")
                    .Ascending()
                .OnColumn("tenant_id");
    }

    public override void Down() =>
        Delete.Table("teams_people");
}
