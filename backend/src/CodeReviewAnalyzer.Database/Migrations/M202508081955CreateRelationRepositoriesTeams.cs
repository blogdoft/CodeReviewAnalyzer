using FluentMigrator;
using FluentMigrator.Postgres;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508081955, description: "Create relation between Teams and Repositories")]
public class M202508081955CreateRelationRepositoriesTeams : Migration
{
    public override void Up()
    {
        Create.Table("teams_repositories")
            .WithColumn("teams_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_repositories_team", "teams", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumnDescription("FK to Team table.")
            .WithColumn("teams_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("repositories_id")
                .AsInt64()
                .NotNullable()
                .ForeignKey("fk_teams_repositories_repositories", "repositories", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumnDescription("Fk to Repositories table.")
            .WithColumn("repositories_external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("External id for late merge.")
            .WithColumn("tenants_id")
                .AsInt16()
                .NotNullable()
                .WithColumnDescription("FK for tenant. Necessary for late merge.");

        Create.Index("idx_sh_teams_repositories_team_external_id")
            .OnTable("teams_repositories")
                .OnColumn("teams_external_id")
                    .Ascending()
                .OnColumn("tenants_id")
                    .Unique()
                    .NullsNotDistinct();

        Create.Index("idx_sh_teams_repositories_repository_external_id")
            .OnTable("teams_repositories")
                .OnColumn("repositories_external_id")
                    .Unique()
                    .NullsNotDistinct()
                .OnColumn("tenants_id")
                    .Unique()
                    .NullsNotDistinct();
    }

    public override void Down() =>
        Delete.Table("teams_repositories");
}
