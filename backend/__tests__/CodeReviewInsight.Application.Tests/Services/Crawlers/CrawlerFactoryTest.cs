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

    [Fact]
    public void Should_CreateAzureDevOpsCrawler_When_DataSourceIsAzureDevops()
    {
        // Given
        var azureFacade = Substitute.For<IAzureFacade>();
        var gitRepoRepo = Substitute.For<IGitRepositoryRepository>();
        var sut = new CrawlerFactory(azureFacade, gitRepoRepo);

        TenantId tenantId = Guid.NewGuid();

        var dataSource = new AzureDevOps(
            name: _faker.Random.Word(),
            devOpsUrl: new Uri(_faker.Internet.Url()),
            pat: Guid.NewGuid().ToString(),
            projects: _faker.Random.WordsArray(3),
            areas: _faker.Random.WordsArray(3),
            active: _faker.Random.Bool());

        // When
        var crawler = sut.Create(tenantId, dataSource);

        // Assert
        crawler.ShouldNotBeNull();
        crawler.ShouldBeOfType<AzureDevOpsCrawler>();
        crawler.ShouldBeAssignableTo<IDataSourceCrawler>();
    }

    [Fact]
    public void Should_ThrowKeyNotFound_When_DataSourceIsNotAzureDevops()
    {
        // Given
        var azureFacade = Substitute.For<IAzureFacade>();
        var gitRepoRepo = Substitute.For<IGitRepositoryRepository>();
        var sut = new CrawlerFactory(azureFacade, gitRepoRepo);

        TenantId tenantId = Guid.NewGuid();

        // DataSource fake que NÃO é AzureDevops
        var other = new DummyDataSourceNotAzure(
            name: _faker.Person.FullName,
            active: _faker.Random.Bool());

        // When + Assert
        Should.Throw<KeyNotFoundException>(() => sut.Create(tenantId, other));
    }

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
