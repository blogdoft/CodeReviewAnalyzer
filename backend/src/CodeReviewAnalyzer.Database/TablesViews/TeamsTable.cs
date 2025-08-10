namespace CodeReviewAnalyzer.Database.TablesViews;

public class TeamsTable
{
    public Guid TenantId { get; init; }

    public required string TenantName { get; init; }

    public Guid SharedKey { get; init; }

    public string? ExternalId { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public bool Active { get; init; } = true;
}
