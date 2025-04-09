using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Integrations.WorkItems;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Globalization;
using AzureWorkItem = Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem;

namespace CodeReviewAnalyzer.AzureDevopsItg.Extensions;

internal static class AzureModelsExtension
{
    private static readonly string[] _relations = [
        "System.LinkTypes.Hierarchy-Forward",
        "System.LinkTypes.Hierarchy-Reverse",
        "System.LinkTypes.Related"];

    private static readonly Dictionary<string, Func<WorkItemDto, WorkItem>> _factories = new()
    {
        {
            "Technical Story",
            dto => new TechnicalStory
            {
                Id = dto.Id?.ToString()!,
                AreaPath = dto.AreaPath,
                Project = dto.Project,
                Title = dto.Title,
                CreatedAt = dto.CreatedAt,
                ClosedAt = dto.ClosedAt,
                RelatedTo = dto.RelatedIds ?? [],
            }
        },
        {
            "User Story",
            dto => new UserStory
            {
                Id = dto.Id?.ToString()!,
                AreaPath = dto.AreaPath,
                Project = dto.Project,
                Title = dto.Title,
                CreatedAt = dto.CreatedAt,
                ClosedAt = dto.ClosedAt,
                RelatedTo = dto.RelatedIds ?? [],
            }
        },
        {
            "Bug",
            dto => new Bug
            {
                Id = dto.Id?.ToString()!,
                AreaPath = dto.AreaPath,
                Project = dto.Project,
                Title = dto.Title,
                CreatedAt = dto.CreatedAt,
                ClosedAt = dto.ClosedAt,
                RelatedTo = dto.RelatedIds ?? [],
            }
        },
        {
            "Defect",
            dto => new Defect
            {
                Id = dto.Id?.ToString()!,
                AreaPath = dto.AreaPath,
                Project = dto.Project,
                Title = dto.Title,
                CreatedAt = dto.CreatedAt,
                ClosedAt = dto.ClosedAt,
                RelatedTo = dto.RelatedIds ?? [],
            }
        },
    };

    public static IntegrationUser ToUser(this IdentityRef identifyRef) => new()
    {
        Id = identifyRef.Id,
        Name = identifyRef.DisplayName,
        Active = !identifyRef.Inactive,
    };

    public static IntegrationUser ToUser(this IdentityRefWithVote identityRef) => new()
    {
        Id = identityRef.Id,
        Name = identityRef.DisplayName,
        Active = !identityRef.Inactive,
    };

    public static WorkItem ToWorkItem(this AzureWorkItem azureWorkItem)
    {
        if (!azureWorkItem.Fields.TryGetValue("System.WorkItemType", out var workItemType))
        {
            throw new ArgumentException("Work item type not found");
        }

        if (!_factories.ContainsKey(workItemType.ToString()!))
        {
            throw new ArgumentException($"Work item type {workItemType} not supported");
        }

        return _factories[workItemType.ToString()!] !(new WorkItemDto(azureWorkItem));
    }

    internal record WorkItemDto
    {
        public WorkItemDto(AzureWorkItem workItem)
        {
            Id = workItem.Id;
            Type = workItem.Fields["System.WorkItemType"].ToString()!;
            Project = workItem.Fields["System.TeamProject"].ToString()!;
            AreaPath = workItem.Fields["System.AreaPath"].ToString()!;
            Title = workItem.Fields["System.Title"].ToString()!;
            CreatedAt = DateTime.Parse(
                workItem.Fields["System.CreatedDate"].ToString()!,
                CultureInfo.InvariantCulture);

            if (workItem.Fields.TryGetValue("System.ChangeDate", out var value) && workItem.Fields["System.State"].ToString() == "Closed")
            {
                ClosedAt = DateTime.Parse(
                    value.ToString()!,
                    CultureInfo.InvariantCulture);
            }

            if (workItem.Relations is not null)
            {
                RelatedIds = workItem.Relations
                    .Where(relation => _relations.Contains(relation.Rel))
                    .Select(relation =>
                    {
                        var splitUrl = relation.Url.Split('/');
                        var relatedId = splitUrl[^1];
                        return relatedId;
                    });
            }
        }

        public int? Id { get; }
        public string Type { get; }
        public string Project { get; }
        public string AreaPath { get; }
        public string Title { get; }
        public DateTime CreatedAt { get; }
        public DateTime? ClosedAt { get; }

        public IEnumerable<string>? RelatedIds { get; }
    }
}
