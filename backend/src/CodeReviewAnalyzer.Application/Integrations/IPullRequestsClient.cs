using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Integrations;

public interface IPullRequestsClient
{
    IAsyncEnumerable<PullRequest> GetPullRequestsAsync(
        Configuration configuration,
        DateTime? minTime,
        DateTime? maxTime = null);
}
