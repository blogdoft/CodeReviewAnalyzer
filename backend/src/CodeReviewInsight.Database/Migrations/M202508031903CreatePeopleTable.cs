using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewInsight.Database.Migrations;

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
            .WithColumn("tenant_id")
                .AsInt16()
                .NotNullable()
                .ForeignKey("fk_people__tenant_id", "tenants", "id")
                    .OnDeleteOrUpdate(System.Data.Rule.None)
                .Indexed("idx_fk_people__tenant_id")
                .WithColumnDescription("Fk to identify wich tenant this person is linked to.")
            .WithColumn("external_id")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("Key used on information source database.")
                .Indexed("idx_sh_people__external_id")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .Indexed("idx_uk_people__shared_key")
                .Unique()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Person's name.")
            .WithColumn("name_sh")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Searcheable name field.")
                .Indexed("idx_sh_people__name_sh")
            .WithColumn("avatar_url")
                .AsString(2048)
                .Nullable()
                .WithColumnDescription("URL that refers to person's avatar.");

    public override void Down() =>
        Delete.Table("people");
}
