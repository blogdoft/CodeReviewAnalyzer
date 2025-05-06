using CodeReviewAnalyzer.Application.Integrations.WorkItems;
using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Integrations;

public interface IWorkItemsIntegration
{
    Task<IList<WorkItem>> GetWorkItemsAsync(
        Configuration configuration,
        DateOnly from,
        DateOnly to,
        IList<string> workItemTypes);

    Task<IList<WorkItem>> GetWorkItemsByIdAsync(
        Configuration configuration,
        IList<string> workItemIds,
        string [] supportedTypes);
}
