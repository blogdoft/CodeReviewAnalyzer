using CodeReviewInsight.Application.Integrations.Models;
using CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;
using CodeReviewInsight.AzureDevopsItg.Clients;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using CodeReviewInsight.Domain.Features.GitRepositories;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Data;
using GitRepository = CodeReviewInsight.Domain.Features.GitRepositories.GitRepository;

namespace CodeReviewInsight.AzureDevopsItg;

public class AzureFacade(ILogger<AzureFacade> logger, IConnectionFactory connectionFactory) : IAzureFacade
{
    private const string MissingTenantError = "You must provide a TenantId when importing git repositories.";
    private const string ProjectDoesNotExists =
        "Project {ProjectName} does not exists with provided name or provided credentials does not have access to it." +
        " Review tenant {DevOpsUrl}-{TenantId} configurations.";

    private string? _projectName;
    private Uri? _devOpsUrl;
    private string? _pat;
    private TenantId? _tenantId;

    public IAzureFacade SetContext(TenantId tenantId, AzureDevOps azureDevOps)
    {
        _devOpsUrl = azureDevOps.DevOpsUrl;
        _pat = azureDevOps.Pat;
        _tenantId = tenantId;
        return this;
    }

    public IAzureFacade FromProject(string projectName)
    {
        _projectName = projectName;
        return this;
    }

    public async Task<IEnumerable<PullRequest>> GetPullRequestsAsync(DateTime startPeriod, DateTime endPeriod)
    {
        using var connection = connectionFactory.CreateConnection(_devOpsUrl!, _pat!);
        using var projectClient = await connection.GetClientAsync<ProjectHttpClient>();
        var project = await projectClient.GetProject(_projectName);

        var gitClient = await connection.GetClientAsync<GitHttpClient>();
        var repositories = await gitClient.GetRepositoriesAsync(project.Id, includeHidden: false);
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
                    MinTime = startPeriod,
                    MaxTime = endPeriod,
                });
        }

        return [];
    }

    public async Task<IEnumerable<GitRepository>> GetRepositoriesAsync()
    {
        try
        {
            using var connection = connectionFactory.CreateConnection(_devOpsUrl!, _pat!);
            using var projectClient = await connection.GetClientAsync<ProjectHttpClient>();
            var project = await projectClient.GetProject(_projectName);

            var gitClient = await connection.GetClientAsync<GitHttpClient>();
            var repositories = await gitClient.GetRepositoriesAsync(project.Id, includeHidden: false);

            return repositories.Select(r => new GitRepositoryFactory()
                .WithId(r.Id)
                .WithExternalId(r.Id.ToString())
                .WithTenant(_tenantId ??
                    throw new NoNullAllowedException(MissingTenantError))
                .WithName(r.Name)
                .WithUrl(r.RemoteUrl)
                .Build());
        }
        catch (ProjectDoesNotExistWithNameException ex)
        {
            logger.LogError(
                exception: ex,
                message: ProjectDoesNotExists,
                _projectName,
                _devOpsUrl,
                _tenantId);
            return [];
        }
    }
}
