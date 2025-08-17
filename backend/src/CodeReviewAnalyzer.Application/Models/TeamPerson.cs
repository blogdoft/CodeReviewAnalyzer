namespace CodeReviewAnalyzer.Application.Models;

public class TeamPerson
{
    public required Person Person { get; init; }

    public required Tenant Tenant { get; init; }

    public required string Role { get; init; }
}
