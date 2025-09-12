using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;

public class AzureDevOpsCrawler : IAzureDataSourceCrawler
{
    private readonly TenantId _tenantId;
    private readonly AzureDevOps _dataSource;
    private readonly IAzureFacade _azureFacade;
    private readonly IGitRepositoryRepository _gitRepositoryRepository;
    private readonly IPullRequests _pullRequestRepository;

    public AzureDevOpsCrawler(
        TenantId tenantId,
        AzureDevOps dataSource,
        IAzureFacade azureFacade,
        IGitRepositoryRepository gitRepositoryRepository,
        IPullRequests pullRequestRepository)
    {
        _tenantId = tenantId;
        _dataSource = dataSource;
        _azureFacade = azureFacade;
        _gitRepositoryRepository = gitRepositoryRepository;
        _pullRequestRepository = pullRequestRepository;
    }

    public async Task CrawAsync()
    {
        _azureFacade
            .SetContext(_tenantId, _dataSource);
        foreach (var project in _dataSource.Projects)
        {
            _azureFacade.FromProject(project);

            await LoadRepositoriesAsync();

            await LoadPullRequestsAsync();
        }
    }

    private async Task LoadRepositoriesAsync()
    {
        var repositories = await _azureFacade.GetRepositoriesAsync();

        await _gitRepositoryRepository.BulkUpsertAsync(repositories);
    }

    private async Task LoadPullRequestsAsync()
    {
        var pullRequests = await _azureFacade.GetPullRequestsAsync(
            startPeriod: DateTime.UtcNow.AddMonths(-3),
            endPeriod: DateTime.UtcNow);

        await _pullRequestRepository.AddRange(pullRequests);
    }
}
