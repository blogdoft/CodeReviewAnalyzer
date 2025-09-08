namespace CodeReviewInsight.Database.TablesViews;

public abstract class DataSourceTable
{
    public string? IntegrationType { get; set; }
}

public class AzureDevOpsTable : DataSourceTable
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }

    public string DevOpsUrl { get; set; } = string.Empty;

    public string? Pat { get; set; }

    public string? Projects { get; set; }

    public string? Areas { get; set; }
}
