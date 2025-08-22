using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508081955, description: "Create relation between Teams and Repositories")]
public class M202508081955CreateRelationRepositoriesTeams : Migration
{
    public override void Up()
    {
        Create.Table("teams_repositories")
            .WithColumn("team_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_repositories__team_id", "teams", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_teams_repositories__team_id")
                .WithColumnDescription("FK to Team table.")
            .WithColumn("team_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("repository_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_repositories__repository_id", "repositories", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_teams_repositories__repository_id")
                .WithColumnDescription("Fk to Repositories table.")
            .WithColumn("repository_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("tenant_id")
                .AsInt16()
                .NotNullable()
                .WithColumnDescription("FK for tenant. Necessary for late merge.");

        Create.Index("idx_sh_teams_repositories__team_external_id")
            .OnTable("teams_repositories")
                .OnColumn("team_external_id")
                    .Ascending()
                .OnColumn("tenant_id");

        Create.Index("idx_sh_teams_repositories__repository_external_id")
            .OnTable("teams_repositories")
                .OnColumn("repository_external_id")
                    .Ascending()
                .OnColumn("tenant_id");
    }

    public override void Down() =>
        Delete.Table("teams_repositories");
}
