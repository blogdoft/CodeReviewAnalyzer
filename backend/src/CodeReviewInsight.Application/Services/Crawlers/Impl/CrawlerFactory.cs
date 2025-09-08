using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;
using CodeReviewInsight.Domain.Enums;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Services.Crawlers.Impl;

public class CrawlerFactory : ICrawlerFactory
{
    private readonly IAzureFacade _azureFacade;
    private readonly IGitRepositoryRepository _gitRepositoryRepository;

    public CrawlerFactory(
        IAzureFacade azureFacade,
        IGitRepositoryRepository gitRepositoryRepository)
    {
        _azureFacade = azureFacade;
        _gitRepositoryRepository = gitRepositoryRepository;
    }

    public IDataSourceCrawler Create(TenantId tenantId, DataSource dataSource)
    {
        if (dataSource.GetIntegrationType() == IntegrationType.AzureDevops)
        {
            return new AzureDevOpsCrawler(
                tenantId,
                (AzureDevOps)dataSource,
                _azureFacade,
                _gitRepositoryRepository);
        }

        throw new KeyNotFoundException();
    }
}
