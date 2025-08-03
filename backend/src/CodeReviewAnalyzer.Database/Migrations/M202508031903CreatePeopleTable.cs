using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031903, description: "Create People Table")]
public class M202508031903CreatePeopleTable : Migration
{
    public override void Up()
    {
        Create.Table("People")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_people")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("tenantId")
                .AsInt16()
                .NotNullable()
                .Indexed("idx_fk_people_tenant")
                .WithColumnDescription("Fk to identify wich tenant this person is linked to.")
            .WithColumn("sharedKey")
                .AsGuid()
                .NotNullable()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Person's name.")
            .WithColumn("avatarUrl")
                .AsString(2048)
                .Nullable()
                .WithColumnDescription("URL that refers to person's avatar.");

        Create.Index("idx_people_shared_key")
            .OnTable("People")
            .OnColumn("sharedKey")
                .Unique();

        Create.ForeignKey("fk_people_tenant")
            .FromTable("People")
            .ForeignColumn("tenantId")
            .ToTable("Tenants")
            .PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("People");
    }
}
