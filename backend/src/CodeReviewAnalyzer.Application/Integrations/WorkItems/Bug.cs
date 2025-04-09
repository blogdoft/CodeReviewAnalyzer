namespace CodeReviewAnalyzer.Application.Integrations.WorkItems;

public class Bug : WorkItem
{
    public override bool IsRework { get; } = true;
}
