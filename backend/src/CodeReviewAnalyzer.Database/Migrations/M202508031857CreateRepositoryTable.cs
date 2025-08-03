using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031857, description: "Create Repository Table")]
public class M202508031857CreateRepositoryTable : Migration
{
    public override void Up()
    {
        Create.Table("Repository")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_repository")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("tenantId")
                .AsInt16()
                .NotNullable()
                .Indexed("idx_fk_repository_tenant")
                .WithColumnDescription("Fk to identify wich tenant this repository belongs to.")
            .WithColumn("sharedKey")
                .AsGuid()
                .NotNullable()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("A descriptive name to identify this team.")
            .WithColumn("url")
                .AsString(2048)
                .Nullable()
                .WithColumnDescription("URL that refers to GIT repository.");

        Create.Index("idx_repository_shared_key")
            .OnTable("Repository")
            .OnColumn("sharedKey")
                .Unique();

        Create.ForeignKey("fk_repository_tenant")
            .FromTable("Repository")
            .ForeignColumn("tenantId")
            .ToTable("Tenants")
            .PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("Repository");
    }
}
