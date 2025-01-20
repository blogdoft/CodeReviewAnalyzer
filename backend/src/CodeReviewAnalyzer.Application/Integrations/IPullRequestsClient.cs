using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Services;

namespace CodeReviewAnalyzer.Application.Integrations;

public interface IPullRequestsClient
{
    IAsyncEnumerable<PullRequest> GetPullRequestsAsync(
        Configuration configuration,
        WorkingHourCalculator workingHourCalculator,
        DateTime? minTime,
        DateTime? maxTime = null);
}
