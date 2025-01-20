using CodeReviewAnalyzer.Application.Integrations;
using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Services;
using CodeReviewAnalyzer.AzureDevopsItg.Factories;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace CodeReviewAnalyzer.AzureDevopsItg.Clients.Impl;

internal sealed class PullRequestClient(IConnectionFactory connectionFactory) : IPullRequestsClient
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;

    public async IAsyncEnumerable<PullRequest> GetPullRequestsAsync(
        Configuration configuration,
        WorkingHourCalculator workingHourCalculator,
        DateTime? minTime,
        DateTime? maxTime = null)
    {
        using var connection = _connectionFactory.CreateConnection(configuration);

        var projectClient = await connection.GetClientAsync<ProjectHttpClient>();
        var project = await projectClient.GetProject(configuration.ProjectName);

        var gitClient = await connection.GetClientAsync<GitHttpClient>();

        var repositories = await gitClient.GetRepositoriesAsync(project.Id, includeHidden: false);

        var pullRequestFactory = new PullRequestFactory(configuration, gitClient, workingHourCalculator);

        foreach (var repository in repositories)
        {
            if (repository.IsDisabled ?? false)
            {
                continue;
            }

            var prs = await gitClient.GetPullRequestsAsync(
                repository.Id,
                new GitPullRequestSearchCriteria()
                {
                    Status = PullRequestStatus.Completed,
                    TargetRefName = "refs/heads/develop",
                    MinTime = minTime,
                    MaxTime = maxTime,
                });

            foreach (var pr in prs)
            {
                var sourceIsMain =
                    pr.SourceRefName.Equals("refs/heads/main", StringComparison.OrdinalIgnoreCase) ||
                    pr.SourceRefName.Equals("refs/heads/master", StringComparison.OrdinalIgnoreCase);
                if (sourceIsMain)
                {
                    continue;
                }

                yield return await pullRequestFactory.CreateAsync(pr, repository);
            }
        }
    }
}
