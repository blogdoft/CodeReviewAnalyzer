using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031810, description: "Create table Teams")]
public class M202508031810CreateTeamsTable : Migration
{
    public override void Up() =>
        Create.Table("teams")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_teams")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("tenants_id")
                .AsInt16()
                .NotNullable()
                .ForeignKey("fk_teams_tenants", "tenants", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_teams_tenants")
                .WithColumnDescription("Fk to identify wich tenant this team belongs to.")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .Indexed("idx_uk_teams_shared_key")
                    .Unique()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("When exists, a external identifier.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("A descriptive name to identify this team.")
            .WithColumn("name_sh")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Searchable name field.")
                .Indexed("idx_sh_teams_name_sh");

    public override void Down() =>
        Delete.Table("teams");
}
