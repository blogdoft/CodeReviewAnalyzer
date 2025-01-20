using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviewAnalyzer.Database.Migrations;

[ExcludeFromCodeCoverage]
[Migration(202502011916, description: "Create DayOff table")]
public class M202502011916CreateDayOffTable : Migration
{
    public override void Up() =>
        Create.Table("DAY_OFF")
            .WithColumn("ID")
                .AsInt32()
                .Identity()
                .PrimaryKey("idx_pk_day_off")
                .NotNullable()
                .WithColumnDescription("Primary Key")
            .WithColumn("EXTERNAL_ID")
                .AsGuid()
                .NotNullable()
                .Unique("idx_uk_day_off_external_id")
                .WithColumnDescription("Auto generated external identifier")
            .WithColumn("DESCRIPTION")
                .AsString(255)
                .NotNullable()
                .WithColumnDescription("Describe the day off")
            .WithColumn("DATE")
                .AsDate()
                .NotNullable()
                .Unique("idx_uk_day_off_date")
                .WithColumnDescription("Specify the day off date")
            .WithColumn("YEAR")
                .AsInt16()
                .NotNullable()
                .WithColumnDescription("Year of the day off, used for query performance improvement")
                .Indexed("idx_sh_day_off_year")
            .WithColumn("MONTH")
                .AsInt16()
                .NotNullable()
                .WithColumnDescription("Month of the day off, used for query performance improvement")
                .Indexed("idx_sh_day_off_month");

    public override void Down() =>
        Delete.Table("DAY_OFF");
}
