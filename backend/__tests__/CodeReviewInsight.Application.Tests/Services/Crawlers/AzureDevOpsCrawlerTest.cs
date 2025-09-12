using CodeReviewInsight.Application.Integrations.Models;
using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.Application.Tests.Services.Crawlers;

public class AzureDevOpsCrawlerTest
{
    private readonly TenantId _tenantId;
    private readonly AzureDevOps _dataSource;

    public AzureDevOpsCrawlerTest()
    {
        _tenantId = new TenantId(Guid.NewGuid());
        _dataSource = (AzureDevOps)TenantFixture.BuildAzureDataSource();
        AzureFacade = Substitute.For<IAzureFacade>();
        AzureFacade
            .FromProject(Arg.Any<string>())
            .Returns(AzureFacade);
        AzureFacade
            .SetContext(Arg.Any<TenantId>(), Arg.Any<AzureDevOps>())
            .Returns(AzureFacade);
        GitRepositoryRepository = Substitute.For<IGitRepositoryRepository>();
        PullRequestsRepository = Substitute.For<IPullRequests>();
    }

    internal IAzureFacade AzureFacade { get; }
    internal IGitRepositoryRepository GitRepositoryRepository { get; }
    internal IPullRequests PullRequestsRepository { get; }

    [Fact]
    public async Task Should_PersistAllRepositories_When_TheresIsProjectConfiguredOnDataSourceAsync()
    {
        // Given
        var crawler = Build();

        // When
        await crawler.CrawAsync();

        // Then
        await AzureFacade.Received(_dataSource.Projects.Count).GetRepositoriesAsync();
        await GitRepositoryRepository
            .Received(_dataSource.Projects.Count)
            .BulkUpsertAsync(Arg.Any<IEnumerable<GitRepository>>());
    }

    [Fact]
    public async Task Should_LoadPullRequestsAsync()
    {
        // Given
        var crawler = Build();

        // When
        await crawler.CrawAsync();

        // Then
        await AzureFacade.Received(_dataSource.Projects.Count).GetPullRequestsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());
        await PullRequestsRepository.Received(_dataSource.Projects.Count).AddRange(Arg.Any<IEnumerable<PullRequest>>());
    }

    private AzureDevOpsCrawler Build() => new(
        _tenantId,
        _dataSource,
        AzureFacade,
        GitRepositoryRepository,
        PullRequestsRepository);
}
