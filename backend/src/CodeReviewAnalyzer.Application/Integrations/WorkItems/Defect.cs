namespace CodeReviewAnalyzer.Application.Integrations.WorkItems;

public class Defect : WorkItem
{
    public override bool IsRework { get; } = true;
}
