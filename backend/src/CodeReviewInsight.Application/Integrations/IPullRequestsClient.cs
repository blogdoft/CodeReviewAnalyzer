using CodeReviewInsight.Application.Integrations.Models;
using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Integrations;

public interface IPullRequestsClient
{
    IAsyncEnumerable<PullRequest> GetPullRequestsAsync(
        Configuration configuration,
        DateTime? minTime,
        DateTime? maxTime = null);
}
