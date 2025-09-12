using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Application.Services.Crawlers;
using CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;
using CodeReviewInsight.Application.Services.Crawlers.Impl;
using CodeReviewInsight.Domain.Enums;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Tests.Services.Crawlers;

public class CrawlerFactoryTest
{
    private readonly Faker _faker = BogusFixture.Get();
    public CrawlerFactoryTest()
    {
        AzureFacade = Substitute.For<IAzureFacade>();
        GitRepoRepo = Substitute.For<IGitRepositoryRepository>();
        PullRequestRepository = Substitute.For<IPullRequests>();
    }

    internal IAzureFacade AzureFacade { get; }
    internal IGitRepositoryRepository GitRepoRepo { get; }
    internal IPullRequests PullRequestRepository { get; }

    [Fact]
    public void Should_CreateAzureDevOpsCrawler_When_DataSourceIsAzureDevops()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var dataSource = new AzureDevOps(
            name: _faker.Random.Word(),
            devOpsUrl: new Uri(_faker.Internet.Url()),
            pat: Guid.NewGuid().ToString(),
            projects: _faker.Random.WordsArray(3),
            areas: _faker.Random.WordsArray(3),
            active: _faker.Random.Bool());

        var crawlerFactory = BuildCrawlerFactory();

        // When
        var crawler = crawlerFactory.Create(tenantId, dataSource);

        // Assert
        crawler.ShouldNotBeNull();
        crawler.ShouldBeOfType<AzureDevOpsCrawler>();
        crawler.ShouldBeAssignableTo<IDataSourceCrawler>();
    }

    [Fact]
    public void Should_ThrowKeyNotFound_When_DataSourceIsNotAzureDevops()
    {
        // Given        
        TenantId tenantId = Guid.NewGuid();
        // DataSource fake que NÃO é AzureDevops
        var other = new DummyDataSourceNotAzure(
            name: _faker.Person.FullName,
            active: _faker.Random.Bool());
        var crawlerFactory = BuildCrawlerFactory();

        // When + Assert
        Should.Throw<KeyNotFoundException>(() => crawlerFactory.Create(tenantId, other));
    }

    private CrawlerFactory BuildCrawlerFactory() => new(
        AzureFacade,
        GitRepoRepo,
        PullRequestRepository);

    private sealed class DummyDataSourceNotAzure : DataSource
    {
        public DummyDataSourceNotAzure(
            string name,
            bool active = true)
            : base(name, active)
        {
        }

        public override IntegrationType GetIntegrationType()
        {
            return (IntegrationType)999;
        }
    }
}
