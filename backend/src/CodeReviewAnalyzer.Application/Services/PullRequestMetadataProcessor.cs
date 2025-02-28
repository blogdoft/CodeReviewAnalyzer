using CodeReviewAnalyzer.Application.Integrations;
using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;

namespace CodeReviewAnalyzer.Application.Services;

public class PullRequestMetadataProcessor(
    IPullRequestsClient pullRequestsClient,
    IConfigurations configurationRepository,
    ICodeRepository codeRepository,
    IUsers users,
    IPullRequests pullRequestRepository)
{
    public async Task ExecuteAsync(DateOnly begin, DateOnly end)
    {
        var configurations = await configurationRepository.GetAllAsync();

        foreach (var configuration in configurations)
        {
            await ProcessConfigurationAsync(configuration);
        }
    }

    private async Task ProcessConfigurationAsync(
        Configuration configuration)
    {
        var pullRequests = pullRequestsClient.GetPullRequestsAsync(
            configuration,
            minTime: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            maxTime: DateTime.Now);

        await foreach (var pullRequest in pullRequests)
        {
            await ProcessRepositoryAsync(pullRequest);
            await ProcessUserAsync(pullRequest);
            await ProcessPullRequestAsync(pullRequest);
        }
    }

    private async Task ProcessRepositoryAsync(PullRequest pullRequest) =>
        await codeRepository.AddAsync(pullRequest.Repository);

    private async Task ProcessUserAsync(PullRequest pullRequest)
    {
        await users.Upsert(pullRequest.CreatedBy);

        var usersUnion = pullRequest.Reviewers
            .Select(r => r.User)
            .Union(pullRequest.Comments.Select(c => c.CommentedBy));

        foreach (var reviewer in usersUnion)
        {
            await users.Upsert(reviewer);
        }
    }

    private async Task ProcessPullRequestAsync(PullRequest pullRequest) =>
        await pullRequestRepository.Add(pullRequest);
}
