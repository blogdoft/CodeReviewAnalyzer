using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewInsight.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031857, description: "Create Repository Table")]
public class M202508031857CreateRepositoriesTable : Migration
{
    public override void Up()
    {
        Create.Table("repositories")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("pk_repositories")
                .NotNullable()
                .WithColumnDescription("Primary Key")
                .Indexed("idx_pk_repositories")
            .WithColumn("tenant_id")
                .AsInt16()
                .NotNullable()
                .ForeignKey("fk_repositories__tenant_id", "tenants", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .Indexed("idx_fk_repositories__tenant_id")
                .WithColumnDescription("Fk to identify wich tenant this repositories belongs to.")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .Indexed("idx_uk_repositories__shared_key").Unique()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("Identifier as in DataSource.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("A descriptive name to identify this team.")
            .WithColumn("url")
                .AsString(2048)
                .Nullable()
                .WithColumnDescription("URL that refers to GIT repositories.");

        Create.Index("idx_uk_repositories_tenant_external_id")
            .OnTable("repositories")
            .OnColumn("tenant_id")
            .Ascending()
            .OnColumn("external_id")
            .Unique();
    }

    public override void Down() =>
        Delete.Table("repositories");
}
