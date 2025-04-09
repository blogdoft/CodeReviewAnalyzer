using CodeReviewAnalyzer.Application.Integrations;
using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.AzureDevopsItg.Factories;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Concurrent;

namespace CodeReviewAnalyzer.AzureDevopsItg.Clients.Impl;

internal sealed class PullRequestClient(IConnectionFactory connectionFactory) : IPullRequestsClient
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;

    public async IAsyncEnumerable<PullRequest> GetPullRequestsAsync(
        Configuration configuration,
        DateTime? minTime,
        DateTime? maxTime = null)
    {
        using var connection = _connectionFactory.CreateConnection(configuration);

        var projectClient = await connection.GetClientAsync<ProjectHttpClient>();
        var project = await projectClient.GetProject(configuration.ProjectName);

        var gitClient = await connection.GetClientAsync<GitHttpClient>();

        var repositories = await gitClient.GetRepositoriesAsync(project.Id, includeHidden: false);

        var pullRequestFactory = new PullRequestFactory(configuration, gitClient);

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

            // Recupera workItems
            ConcurrentDictionary<int, string[]> workItems = new();

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 10, // 🔧 limite de chamadas simultâneas (ajuste conforme necessário)
            };

            await Parallel.ForEachAsync(prs, options, async (pr, cancellationToken) =>
            {
                var refs = await gitClient.GetPullRequestWorkItemRefsAsync(
                    project.Id,
                    repository.Id.ToString(),
                    pr.PullRequestId);

                workItems.TryAdd(
                    pr.PullRequestId,
                    refs.Select(reference => reference.Id).ToArray());
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

                yield return await pullRequestFactory.CreateAsync(pr, repository, workItems);
            }
        }
    }
}
