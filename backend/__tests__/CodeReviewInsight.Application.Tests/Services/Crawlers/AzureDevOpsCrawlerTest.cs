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
        AzureProject = Substitute.For<IAzureFacade>();
        AzureProject
            .FromProject(Arg.Any<string>())
            .Returns(AzureProject);
        AzureProject
            .SetContext(Arg.Any<TenantId>(), Arg.Any<AzureDevOps>())
            .Returns(AzureProject);
        GitRepositoryRepository = Substitute.For<IGitRepositoryRepository>();
    }

    internal IAzureFacade AzureProject { get; }
    internal IGitRepositoryRepository GitRepositoryRepository { get; }

    [Fact]
    public async Task Should_PersistAllRepositories_When_TheresIsProjectConfiguredOnDataSourceAsync()
    {
        // Given
        var crawler = Build();

        // When
        await crawler.CrawAsync();

        // Then
        await AzureProject.Received(_dataSource.Projects.Count).GetRepositoriesAsync();
        await GitRepositoryRepository
            .Received(_dataSource.Projects.Count)
            .BulkUpsertAsync(Arg.Any<IEnumerable<GitRepository>>());
    }

    private AzureDevOpsCrawler Build() => new(
        _tenantId,
        _dataSource,
        AzureProject,
        GitRepositoryRepository);
}
