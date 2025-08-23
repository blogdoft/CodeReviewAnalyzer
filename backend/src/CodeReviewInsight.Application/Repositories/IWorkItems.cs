using CodeReviewInsight.Application.Integrations.WorkItems;

namespace CodeReviewInsight.Application.Repositories;

public interface IWorkItems
{
    Task UpsertAsync(WorkItem workItem);

    Task UpsertRelationAsync(HashSet<(string Left, string Right)> bidirecionalPair);
}
