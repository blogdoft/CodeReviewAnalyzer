namespace CodeReviewInsight.Database.TablesViews;

public class TenantTable
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Active { get; set; }

    public List<DataSourceTable> DataSources { get; set; } = null!;
}
