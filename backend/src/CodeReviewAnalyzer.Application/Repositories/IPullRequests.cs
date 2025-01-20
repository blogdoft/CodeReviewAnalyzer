using CodeReviewAnalyzer.Application.Integrations.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface IPullRequests
{
    Task Add(PullRequest pullRequest);
}
