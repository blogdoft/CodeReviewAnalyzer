using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface IPullRequests
{
    Task Add(PullRequest pullRequest);

    Task<PullRequestStats?> GetStatsFromAsync(string externalId);
}
