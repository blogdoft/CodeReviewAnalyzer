using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;

public class AzureDevOpsCrawler : IDataSourceCrawler
{
    private readonly TenantId _tenantId;
    private readonly AzureDevOps _dataSource;
    private readonly IAzureFacade _azureFacade;
    private readonly IGitRepositoryRepository _gitRepositoryRepository;

    public AzureDevOpsCrawler(
        TenantId tenantId,
        AzureDevOps dataSource,
        IAzureFacade azureFacade,
        IGitRepositoryRepository gitRepositoryRepository)
    {
        _tenantId = tenantId;
        _dataSource = dataSource;
        _azureFacade = azureFacade;
        _gitRepositoryRepository = gitRepositoryRepository;
    }

    public async Task CrawAsync()
    {
        foreach (var project in _dataSource.Projects)
        {
            var repositories = await _azureFacade.GetRepositoriesAsync();
            await _gitRepositoryRepository.BulkUpsertAsync(repositories);
        }
    }
}
