using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Integrations;

public interface IGetPullRequestsRelatedWorkItems
{
    Task<RelatedWorkItem> RequestRelatedWorkItems(
        Configuration configuration,
        DateOnly from,
        DateOnly to);
}
