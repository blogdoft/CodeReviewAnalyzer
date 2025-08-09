using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202508031903, description: "Create \"people\" Table")]
public class M202508031903CreatepeopleTable : Migration
{
    public override void Up() =>
        Create.Table("people")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_people")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("tenants_id")
                .AsInt16()
                .NotNullable()
                .ForeignKey("fk_people_tenants", "tenants", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.None)
                .Indexed("idx_fk_people_tenant")
                .WithColumnDescription("Fk to identify wich tenant this person is linked to.")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .Indexed("idx_uk_people_shared_key")
                .Unique()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Person's name.")
            .WithColumn("avatar_url")
                .AsString(2048)
                .Nullable()
                .WithColumnDescription("URL that refers to person's avatar.");

    public override void Down() =>
        Delete.Table("people");
}
