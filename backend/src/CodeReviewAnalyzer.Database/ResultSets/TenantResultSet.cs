namespace CodeReviewAnalyzer.Database.ResultSets;

public class TenantResultSet
{
    public Guid SharedKey { get; init; }

    public string Name { get; init; } = string.Empty;

    public bool Active { get; init; }
}
