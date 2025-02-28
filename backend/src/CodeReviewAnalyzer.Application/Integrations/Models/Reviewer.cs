namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class Reviewer
{
    public required User User { get; init; }

    public short Vote { get; init; } = 0;
}
