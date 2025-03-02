using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202501262002, description: "Create User table")]
public class M202501262002CreateConfigurationTable : Migration
{
    public override void Down() =>
        Delete.Table("USERS");

    public override void Up()
    {
        Create.Table("USERS")
            .WithColumn("ID")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_users")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("EXTERNAL_IDENTIFIER")
                .AsString(size: 255)
                .NotNullable()
                .Unique("idx_uk_users_key")
                .WithColumnDescription("Unique identifier provided by source")
            .WithColumn("NAME")
                .AsString(size: 255)
                .NotNullable()
                .WithColumnDescription("User name.")
            .WithColumn("NAME_SH")
                .AsString(size: 255)
                .NotNullable()
                .Indexed("idx_sh_users_name_sh")
                .WithColumnDescription("User name optimized for queries.")
            .WithColumn("ACTIVE")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true)
                .WithColumnDescription("Flag indicating if user is active.");
    }
}
