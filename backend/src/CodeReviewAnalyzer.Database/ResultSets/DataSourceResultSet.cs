using CodeReviewInsight.Domain.Enums;

namespace CodeReviewAnalyzer.Database.ResultSets;

public class DataSourceResultSet
{
    public string Name { get; init; } = string.Empty;

    public bool Active { get; init; }

    public IntegrationType IntegrationType { get; init; }

    public string DevOpsUrl { get; init; } = string.Empty;

    public string Pat { get; init; } = string.Empty;

    public string Projects { get; init; } = "[]";

    public string Areas { get; init; } = "[]";
}
