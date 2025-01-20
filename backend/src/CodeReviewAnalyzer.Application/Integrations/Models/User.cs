namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class User
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required bool Active { get; init; } = true;
}
