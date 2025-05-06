using CodeReviewAnalyzer.Application.Integrations;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.AzureDevopsItg.Clients;
using CodeReviewAnalyzer.AzureDevopsItg.Extensions;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using AzureWorkItem = Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem;
using WorkItem = CodeReviewAnalyzer.Application.Integrations.WorkItems.WorkItem;

namespace CodeReviewAnalyzer.AzureDevopsItg.Services;

public class WorkItemIntegration(
    IConnectionFactory connectionFactory) : IWorkItemsIntegration
{
    private readonly string[] _ignorableStatus = ["Canceled"];

    public async Task<IList<WorkItem>> GetWorkItemsAsync(
        Configuration configuration,
        DateOnly from,
        DateOnly to,
        IList<string> workItemTypes)
    {
        using var workItemClient = await BuildWorkItemAsync(configuration);

        var workItemsType = string.Join(", ", workItemTypes.Select(type => $"'{type}'"));
        var ignorableStatus = string.Join(", ", _ignorableStatus.Select(type => $"'{type}'"));

        // var areasPath = string.Join(", ", configuration.AreaPath.Select(path => $"'{path}'"));
        var areasPath = @"'AMBEV-SupplyChain-GlobalTechHub\OMNIA\OMNIA Performance Brewhouse\OMNIA Performance Brewhouse Squad'";
        var query = $"""
                
                Select [System.Id] 
                From WorkItems 
                Where [System.WorkItemType] IN ({workItemsType})
                  and [System.State] not in ({ignorableStatus})
                  AND [System.AreaPath] in ({areasPath}) 
                  AND (
                        ([System.CreatedDate] >= '{from:yyyy-MM-dd}' AND [System.CreatedDate] <= '{to:yyyy-MM-dd}')
                        OR
                        ([System.ChangedDate] >= '{from:yyyy-MM-dd}' AND [System.ChangedDate] <= '{to:yyyy-MM-dd}')
                      )
                
            """;
        var wiql = new Wiql()
        {
            Query = query,
        };

        var queryResult = await workItemClient!.QueryByWiqlAsync(wiql, configuration?.ProjectName);

        List<WorkItem> result = new(queryResult.WorkItems.Count());

        foreach (var reworkItemReference in queryResult.WorkItems)
        {
            var reworkItem = await workItemClient!.GetWorkItemAsync(
                reworkItemReference.Id,
                expand: WorkItemExpand.Relations);
            if (reworkItem is null)
            {
                continue;
            }

            if (!IsSupportedType(reworkItem, [.. workItemTypes]))
            {
                continue;
            }

            result.Add(reworkItem.ToWorkItem());
        }

        return result;
    }

    public async Task<IList<WorkItem>> GetWorkItemsByIdAsync(
        Configuration configuration,
        IList<string> workItemIds,
        string[] supportedTypes)
    {
        using var workItemClient = await BuildWorkItemAsync(configuration);

        var foundWorkItems = await workItemClient.GetWorkItemsAsync(
            workItemIds.Select(id => int.Parse(id)),
            expand: WorkItemExpand.Relations);

        return foundWorkItems
            .Where(wi => IsSupportedType(wi, supportedTypes))
            .Select(wi => wi.ToWorkItem())
            .ToList();
    }

    private async Task<WorkItemTrackingHttpClient> BuildWorkItemAsync(
        Configuration configuration)
    {
        var connection = connectionFactory.CreateConnection(configuration);
        return await connection!.GetClientAsync<WorkItemTrackingHttpClient>();
    }

    private bool IsSupportedType(AzureWorkItem workItem, string[] supportedTypes)
    {
        var hasWorkItemType = workItem.Fields.TryGetValue("System.WorkItemType", out var workItemType);
        if (!hasWorkItemType)
        {
            return false;
        }

        var supportedWorkItemType = supportedTypes.Contains(workItemType?.ToString() ?? string.Empty);
        return supportedWorkItemType;
    }
}
