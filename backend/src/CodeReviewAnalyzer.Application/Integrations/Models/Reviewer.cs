namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class Reviewer
{
    public required IntegrationPerson User { get; init; }

    public short Vote { get; init; } = 0;
}
