using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202501181805, description: "Create configuration table")]
public class M202501181805CreateConfigurationTable : Migration
{
    public override void Down() =>
        Delete.Table("CONFIGURATION");

    public override void Up()
    {
        this.Execute.Sql(
            """
                CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
            
            """);
        Create.Table("CONFIGURATION")
            .WithColumn("ID")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_configuration")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("CONFIGURATION_ID")
                .AsGuid()
                .NotNullable()
                .Unique("idx_uk_configuration_id")
                .WithColumnDescription("For navigation purposes")
            .WithColumn("ORGANIZATION")
                .AsString(size: 255)
                .NotNullable()
                .Indexed("idx_sh_configuration_organization")
                .WithColumnDescription("Azure Organization, used to compose azure devops url.")
            .WithColumn("PROJECT_NAME")
                .AsString(size: 255)
                .NotNullable()
                .WithColumnDescription("Azure project name")
            .WithColumn("PERSONAL_ACCESS_TOKEN")
                .AsString(516)
                .WithColumnDescription("Token to access Azure Devops.");
    }
}
