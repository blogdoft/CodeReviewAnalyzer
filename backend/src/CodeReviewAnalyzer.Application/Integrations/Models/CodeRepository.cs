namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class CodeRepository
{
    public required string ExternalId { get; init; }

    public required string Name { get; init; }

    public required string Url { get; init; }
}
