using CodeReviewInsight.Application.Services.Crawlers;
using CodeReviewInsight.Application.Services.Processors.Impl;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Tests.Services.Processors;

public class DataSourceProcessorTest
{
    public DataSourceProcessorTest()
    {
        CrawlerFactory = Substitute.For<ICrawlerFactory>();
        Crawler = Substitute.For<IDataSourceCrawler>();
    }

    internal ICrawlerFactory CrawlerFactory { get; }
    internal IDataSourceCrawler Crawler { get; }

    [Fact]
    public async Task Should_ExtractRepositoriesFromDataSourceAsync()
    {
        // Given
        var tenant = TenantFixture.Build();
        CrawlerFactory
            .Create(Arg.Any<TenantId>(), Arg.Any<DataSource>())
            .Returns(Crawler);
        var processor = Build();

        // When
        await processor.ProcessAsync(tenant);

        // Then
        CrawlerFactory.Received(tenant.DataSource.Count()).Create(tenant.Id, Arg.Any<DataSource>());
        await Crawler.Received(tenant.DataSource.Count()).CrawAsync();
    }

    private DataSourceProcessor Build() => new(CrawlerFactory);
}
