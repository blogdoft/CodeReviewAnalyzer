using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Integrations.Models;

public class RelatedWorkItem
{
    public required string PullRequestId { get; init; }

    public required string WorkItemType { get; init; }

    public required string WorkItemId { get; init; }

    public required string AreaPath { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? ClosedAt { get; init; }

    public Person? User { get; init; }
}
