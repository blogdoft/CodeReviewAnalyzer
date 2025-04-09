using CodeReviewAnalyzer.Application.Integrations.WorkItems;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;
using Microsoft.Extensions.Logging;

namespace CodeReviewAnalyzer.Database.Repositories;

internal class WorkItemsRepository : IWorkItems
{
    private const string Upsert =
        """
            INSERT INTO public."WORKITEMS" (
                  external_id
                , type
                , area_path
                , project
                , title
                , created_at
                , closed_at
            ) VALUES(
                  @ExternalId
                , @Type
                , @AreaPath
                , @Project
                , @Title
                , @CreatedAt
                , @ClosedAt
            ) 
            ON CONFLICT (external_id,project) DO 
            UPDATE SET 
                  area_path=@AreaPath
                , title=@Title
                , created_at=@CreatedAt
                , closed_at=@ClosedAt
            returning id;

        """;

    private const string UpsertRelation =
        """
            INSERT INTO public."WORKITEMS_RELATION" (
                  left_workitem_id
                , right_workitem_id
            ) SELECT (select left_t.id from "WORKITEMS" as left_t where left_t.external_id = @leftId)
                   , (select right_t.id from "WORKITEMS" as right_t where right_t.external_id = @rightId) 
            ON CONFLICT (left_workitem_id, right_workitem_id) 
            DO NOTHING;
            
        """;

    private readonly ILogger<WorkItemsRepository> _logger;
    private readonly IDatabaseFacade _databaseFacade;

    public WorkItemsRepository(
        ILogger<WorkItemsRepository> logger,
        IDatabaseFacade databaseFacade)
    {
        _logger = logger;
        _databaseFacade = databaseFacade;
    }

    public async Task UpsertAsync(WorkItem workItem)
    {
        var id = await _databaseFacade.ExecuteScalarAsync<int>(Upsert, new
        {
            ExternalId = workItem.Id,
            Type = workItem.GetType().Name,
            workItem.AreaPath,
            workItem.Project,
            workItem.Title,
            workItem.CreatedAt,
            workItem.ClosedAt,
        });
        _logger.LogInformation("Upserting work item with ID: {Id}, type: {Type}", workItem.Id, Upsert);

        await SyncPullRequestWorkItemAsync(id, workItem.Id);
    }

    public async Task UpsertRelationAsync(HashSet<(string Left, string Right)> bidirecionalPair)
    {
        foreach (var (leftId, rightId) in bidirecionalPair)
        {
            await _databaseFacade.ExecuteScalarAsync<int>(UpsertRelation, new
            {
                leftId,
                rightId,
            });
            _logger.LogInformation("Upserting work item relations: {LeftId}-{RightId}", leftId, rightId);
        }
    }

    private async Task SyncPullRequestWorkItemAsync(int workItemId, string externalId)
    {
        await _databaseFacade.ExecuteAsync(PullRequestWorkitemsStmt.SyncByExternalId, new
        {
            WorkItemId = workItemId,
            ExternalId = externalId,
        });
    }
}
