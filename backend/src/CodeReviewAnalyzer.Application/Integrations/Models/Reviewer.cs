namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class Reviewer
{
    public required IntegrationUser User { get; init; }

    public short Vote { get; init; } = 0;
}
