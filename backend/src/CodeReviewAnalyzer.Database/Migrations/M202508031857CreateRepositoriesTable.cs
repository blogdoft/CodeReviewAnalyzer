using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031857, description: "Create Repository Table")]
public class M202508031857CreateRepositoriesTable : Migration
{
    public override void Up() =>
        Create.Table("repositories")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_repositories")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("tenants_id")
                .AsInt16()
                .NotNullable()
                .ForeignKey("fk_repositories_tenants", "tenants", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_repositories_tenant")
                .WithColumnDescription("Fk to identify wich tenant this repositories belongs to.")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .Indexed("idx_uk_repositories_shared_key").Unique()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("A descriptive name to identify this team.")
            .WithColumn("url")
                .AsString(2048)
                .Nullable()
                .WithColumnDescription("URL that refers to GIT repositories.");

    public override void Down() =>
        Delete.Table("repositories");
}
