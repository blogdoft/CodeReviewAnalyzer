using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202502221318, description: "Create Team table")]
public class M202502221318CreateTeamTable : Migration
{
    public override void Up()
    {
        CreateTeams();
        CreateTeamsRelations();
    }

    public override void Down() =>
        Delete.Table("TEAMS");

    private void CreateTeams() =>
        Create.Table("TEAMS")
            .WithColumn("id")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_teams")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("external_id")
                .AsString(255)
                .NotNullable()
                .Unique("idx_uk_teams_external_id")
                .WithColumnDescription("Auto generated external identifier")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Friendly team identifier")
            .WithColumn("name_sh")
                .AsString(255)
                .NotNullable()
                .Indexed("idx_sh_teams_name")
                .WithColumnDescription("For query purpose.")
            .WithColumn("description")
                .AsString(2000)
                .Nullable()
                .WithColumnDescription("Describe the team in a feel words.")
            .WithColumn("active")
                .AsBoolean()
                .NotNullable()
                .WithColumnDescription("Team still working or not")
                .WithDefaultValue(true);

    private void CreateTeamsRelations()
    {
        Create.Table("TEAM_USER")
            .WithDescription("This is a relational table between user and team")
            .WithColumn("team_id")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Team pk")
            .WithColumn("user_id")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("User PK")
            .WithColumn("role_in_team")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("The users' team role. E.g.: Developer, QA, Tech Lead")
            .WithColumn("joined_at_utc")
                .AsDateTime()
                .WithColumnDescription("The date when user join to team.");

        Create
            .PrimaryKey("PK_TEAM_USER")
            .OnTable("TEAM_USER")
                .Columns("team_id", "user_id");

        Create
            .ForeignKey("idx_teamuser_users")
            .FromTable("TEAM_USER")
                .ForeignColumn("user_id")
            .ToTable("USERS")
                .PrimaryColumn("ID");

        Create
            .ForeignKey("idx_teamuser_team")
            .FromTable("TEAM_USER")
                .ForeignColumn("team_id")
            .ToTable("TEAMS")
                .PrimaryColumn("id");

        Create.Table("TEAM_REPOSITORY")
            .WithDescription("This is a relational table between repository and team")
            .WithColumn("team_id")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Team pk")
            .WithColumn("repository_id")
                .AsInt32()
                .NotNullable()
                .WithColumnDescription("Repository PK");

        Create
            .PrimaryKey("PK_TEAM_REPOSITORY")
            .OnTable("TEAM_REPOSITORY")
                .Columns("team_id", "repository_id");

        Create
            .ForeignKey("idx_teamrepository_repositories")
            .FromTable("TEAM_REPOSITORY")
                .ForeignColumn("repository_id")
            .ToTable("REPOSITORIES")
                .PrimaryColumn("id");

        Create
            .ForeignKey("idx_teamrepository_teams")
            .FromTable("TEAM_REPOSITORY")
                .ForeignColumn("team_id")
            .ToTable("TEAMS")
                .PrimaryColumn("id");

        // PROJECTS table
        Create.Table("PROJECTS")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumnDescription("Unique identifier for the project. Example: 1")
            .WithColumn("external_id").AsGuid().NotNullable().Unique("idx_uk_projects_external_id")
                .WithColumnDescription("Used for API and external key.")
            .WithColumn("team_id").AsInt32().NotNullable()
                .WithColumnDescription("Associated team ID. Example: 1")
            .WithColumn("name").AsString(100).NotNullable()
                .WithColumnDescription("Name of the project. Example: 'Project X'")
            .WithColumn("description").AsString(1000).Nullable()
                .WithColumnDescription("Project description. Example: 'A project for developing a REST API'")
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumnDescription("Project creation date. Example: '2023-02-21T00:00:00Z'")
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumnDescription("Project last update date. Example: '2023-02-21T00:00:00Z'");
        Create
            .ForeignKey("FK_PROJECTS_TEAMS")
            .FromTable("PROJECTS").ForeignColumn("team_id")
            .ToTable("TEAMS").PrimaryColumn("id");
    }
}
