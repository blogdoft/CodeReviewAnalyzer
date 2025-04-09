namespace CodeReviewAnalyzer.Application.Integrations.WorkItems;

public abstract class WorkItem
{
    public string WorkItemType
    {
        get
        {
            return this.GetType().Name;
        }
    }

    public required string Id { get; init; }

    public required string AreaPath { get; init; }

    public required string Project { get; init; }

    public required string Title { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? ClosedAt { get; init; }

    public IEnumerable<string> RelatedTo { get; init; } = [];

    public virtual bool IsRework { get; } = false;
}
