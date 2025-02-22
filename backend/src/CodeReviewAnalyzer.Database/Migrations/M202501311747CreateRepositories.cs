using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202501311747, description: "Create Pull Request structure")]
public class M202501311747CreatePullRequestTable : Migration
{
    public override void Up()
    {
        // REPOSITORIES table
        Create.Table("REPOSITORIES")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumnDescription("Unique identifier for the repository.")
            .WithColumn("external_id").AsString(255).NotNullable().Unique("idx_uk_repositories_external_id")
                .WithColumnDescription("Used for API and external key.")
            .WithColumn("name").AsString(100).NotNullable()
                .WithColumnDescription("Repository name. Example: 'repo-api'")
            .WithColumn("remote_url").AsString(1000).Nullable()
                .WithColumnDescription("Remote repository URL. Example: 'https://github.com/company/repo-api'")
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumnDescription("Repository creation date. Example: '2023-02-21T00:00:00Z'")
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumnDescription("Repository last update date. Example: '2023-02-21T00:00:00Z'");
    }

    public override void Down()
    {
        Delete.Table("REPOSITORIES");
    }
}
