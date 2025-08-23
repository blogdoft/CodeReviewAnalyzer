using CodeReviewInsight.Application.Integrations.Models;
using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Integrations;

public interface IGetPullRequestsRelatedWorkItems
{
    Task<RelatedWorkItem> RequestRelatedWorkItems(
        Configuration configuration,
        DateOnly from,
        DateOnly to);
}
