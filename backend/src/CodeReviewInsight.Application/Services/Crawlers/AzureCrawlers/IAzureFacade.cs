using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;

public interface IAzureFacade
{
    IAzureFacade FromProject(string projectName);

    IAzureFacade SetContext(TenantId tenantId, AzureDevOps azureDevOps);

    Task<IEnumerable<GitRepository>> GetRepositoriesAsync();
}
