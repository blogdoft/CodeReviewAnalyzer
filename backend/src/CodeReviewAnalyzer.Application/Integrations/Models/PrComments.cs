namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class PrComments
{
    public required int CommentIndex { get; init; }

    public required int ThreadId { get; init; }

    public required IntegrationUser CommentedBy { get; init; }

    public required string Comment { get; init; }

    public required DateTime CommentDate { get; init; }

    public required DateTime ResolvedDate { get; init; }
}
