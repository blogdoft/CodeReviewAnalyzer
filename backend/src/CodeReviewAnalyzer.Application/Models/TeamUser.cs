namespace CodeReviewAnalyzer.Application.Models;

public class TeamUser
{
    public required User User { get; init; }

    public required string Role { get; init; }
}
