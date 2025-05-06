using CodeReviewAnalyzer.Application.Integrations;
using CodeReviewAnalyzer.Application.Integrations.WorkItems;
using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Repositories;

namespace CodeReviewAnalyzer.Application.Services.Crawlers.Impl;

public class WorkItemsCrawler : IWorkItemsCrawler
{
    private readonly IConfigurations _configurations;
    private readonly IWorkItemsIntegration _workItemsItg;
    private readonly IWorkItems _workItems;

    public WorkItemsCrawler(
        IConfigurations configurations,
        IWorkItemsIntegration workItemsCrawler,
        IWorkItems workItems)
    {
        _configurations = configurations;
        _workItemsItg = workItemsCrawler;
        _workItems = workItems;
    }

    public async Task CrawAsync(ReportFilter filter)
    {
        string[] supportedTypes = ["Defect", "Bug", "User Story", "Technical Story"];
        var configuration = await _configurations.GetAllAsync();
        var reworkWorkItem = await _workItemsItg.GetWorkItemsAsync(
            configuration.First(),
            filter.From,
            filter.To,
            supportedTypes);
        await PersistAsync(reworkWorkItem);

        var relatedWorkItem = await _workItemsItg.GetWorkItemsByIdAsync(
            configuration.First(),
            [.. reworkWorkItem.SelectMany(wi => wi.RelatedTo)],
            supportedTypes);
        await PersistAsync(relatedWorkItem);

        await BuildWorkItemsRelationAsync(reworkWorkItem, relatedWorkItem);
    }

    private async Task PersistAsync(IEnumerable<WorkItem> workItems)
    {
        foreach (var workItem in workItems)
        {
            await _workItems.UpsertAsync(workItem);
        }
    }

    private async Task BuildWorkItemsRelationAsync(
        IEnumerable<WorkItem> reworkWorkItem,
        IEnumerable<WorkItem> relatedWorkItem)
    {
        var joinedList = reworkWorkItem.Concat(relatedWorkItem);
        var workItemDict = joinedList
            .DistinctBy(w => w.Id)
            .ToDictionary(w => w.Id);
        var bidirecionalPair = new HashSet<(string, string)>(joinedList.Count());

        foreach (var workItem in joinedList)
        {
            foreach (var relatedId in workItem.RelatedTo)
            {
                if (workItemDict.TryGetValue(relatedId, out var relatedItem))
                {
                    if (!relatedItem.RelatedTo.Contains(workItem.Id))
                    {
                        continue;
                    }

                    var orderedPair = workItem.Id.CompareTo(relatedId) switch
                    {
                        < 0 => (relatedId, workItem.Id),
                        > 0 => (workItem.Id, relatedId),
                        _ => (workItem.Id, relatedId)
                    };

                    bidirecionalPair.Add(orderedPair);
                }
            }
        }

        await _workItems.UpsertRelationAsync(bidirecionalPair);
    }
}
