using CodeReviewInsight.Application.Integrations.Models;
using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Repositories;

public interface IPullRequests
{
    Task Add(PullRequest pullRequest);

    Task<PullRequestStats?> GetStatsFromAsync(string externalId);
}
