using CodeReviewAnalyzer.Application.Integrations.WorkItems;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface IWorkItems
{
    Task UpsertAsync(WorkItem workItem);

    Task UpsertRelationAsync(HashSet<(string Left, string Right)> bidirecionalPair);
}
