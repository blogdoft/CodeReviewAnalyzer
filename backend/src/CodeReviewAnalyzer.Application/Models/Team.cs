namespace CodeReviewAnalyzer.Application.Models;

public class Team
{
    public required Guid ExternalId { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public bool Active { get; init; } = true;
}
