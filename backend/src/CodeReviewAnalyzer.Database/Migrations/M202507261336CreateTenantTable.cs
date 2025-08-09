using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202507261336, description: "Create table to control tenants")]
public class M202507261336CreateTenantTable : Migration
{
    public override void Up()
    {
        Create.Table("tenants")
            .WithColumn("id")
                .AsInt16()
                .Identity()
                .PrimaryKey("idx_pk_tentant")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("shared_key")
                .AsGuid()
                .NotNullable()
                .WithColumnDescription("Key to be shared with domains/urls.")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("A descriptive name to identify this tenant.")
            .WithColumn("active")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true)
                .WithColumnDescription("Check if any operation under this tenant could be completed/executed");

        Create.Index("idx_tenants_shared_key")
            .OnTable("tenants")
            .OnColumn("shared_key")
                .Unique();

        CreateDataSource();
    }

    public override void Down()
    {
        Delete.ForeignKey("idx_fk_azure_devops_data_source").OnTable("azure_devops");
        Delete.Table("azure_devops");
        Delete.Index("idx_data_source_shared_key").OnTable("data_source");
        Delete.Table("data_source");
        Delete.Index("idx_tenants_shared_key").OnTable("tenants");
        Delete.Table("tenants");
    }

    private void CreateDataSource()
    {
        Create.Table("data_sources")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_data_source")
                .NotNullable()
                .WithColumnDescription("Primary key")
            .WithColumn("tenants_id")
                .AsInt64()
                .NotNullable()
                .WithColumnDescription("Foreign key to Tenant")
            .WithColumn("name")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("A descriptive name to identify this tenant.")
            .WithColumn("active")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true)
                .WithColumnDescription("Check if any operation under this tenant could be completed/executed")
            .WithColumn("integration_type")
                .AsString(50)
                .NotNullable()
                .WithColumnDescription("What kind of integration this seupt is.");

        Create.ForeignKey("idx_fk_data_source_tenants")
        .FromTable("data_sources").ForeignColumn("tenants_id")
        .ToTable("tenants").PrimaryColumn("id")
        .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        CreateAzureDevops();
    }

    private void CreateAzureDevops()
    {
        Create.Table("azure_devops")
            .WithColumn("id")
                .AsInt64()
                .PrimaryKey()
                .NotNullable()
                .WithColumnDescription("Reference to dataSource")
            .WithColumn("devops_url")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("The company azure devops url: https://dev.azure.com/COMPANY-NAME")
            .WithColumn("pat")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Used for automatic connection between application and Az DevOps")
            .WithColumn("projects")
                .AsCustom("TEXT")
                .Nullable()
                .WithColumnDescription("Store a json array with projects names")
            .WithColumn("areas")
                .AsCustom("TEXT")
                .Nullable()
                .WithColumnDescription("Store a json array with areas");

        Create.ForeignKey("idx_fk_azure_devops_data_source")
            .FromTable("azure_devops").ForeignColumn("id")
            .ToTable("data_sources").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}
