using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202507261336, description: "Create table to control tenants")]
public class M202507261336CreateTenantTable : Migration
{
    public override void Up()
    {
        Create.Table("Tenants")
            .WithColumn("id")
                .AsInt16()
                .Identity()
                .PrimaryKey("idx_pk_tentant")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("sharedKey")
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
            .OnTable("Tenants")
            .OnColumn("sharedKey")
                .Unique();

        CreateDataSource();
    }

    public override void Down()
    {
        Delete.ForeignKey("idx_fk_AzureDevops_DataSource").OnTable("AzureDevops");
        Delete.Table("AzureDevops");
        Delete.Index("idx_dataSource_sharedkey").OnTable("DataSource");
        Delete.Table("DataSource");
        Delete.Index("idx_tenants_shared_key").OnTable("Tenants");
        Delete.Table("Tenants");
    }

    private void CreateDataSource()
    {
        Create.Table("DataSource")
            .WithColumn("id")
                .AsInt64()
                .Identity()
                .PrimaryKey("idx_pk_dataSource")
                .NotNullable()
                .WithColumnDescription("Primary key")
            .WithColumn("tenant_id")
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
            .WithColumn("integrationType")
                .AsString(50)
                .NotNullable()
                .WithColumnDescription("What kind of integration this seupt is.");

        CreateAzureDevops();
    }

    private void CreateAzureDevops()
    {
        Create.Table("AzureDevops")
            .WithColumn("id")
                .AsInt64()
                .PrimaryKey()
                .NotNullable()
                .WithColumnDescription("Reference to dataSource")
            .WithColumn("devopsUrl")
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

        Create.ForeignKey("idx_fk_AzureDevops_DataSource")
            .FromTable("AzureDevops").ForeignColumn("id")
            .ToTable("DataSource").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}
