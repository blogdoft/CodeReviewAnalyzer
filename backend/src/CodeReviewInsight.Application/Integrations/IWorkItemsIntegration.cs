using CodeReviewInsight.Application.Integrations.WorkItems;
using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Integrations;

public interface IWorkItemsIntegration
{
    Task<IList<WorkItem>> GetWorkItemsAsync(
        Configuration configuration,
        DateOnly from,
        DateOnly to,
        IList<string> workItemTypes);

    Task<IList<WorkItem>> GetWorkItemsByIdAsync(
        Configuration configuration,
        IList<string> workItemIds);
}
