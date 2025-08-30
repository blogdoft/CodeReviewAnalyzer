using CodeReviewInsight.Application.Services.Crawlers;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Services.Processors.Impl;

public class DataSourceProcessor(ICrawlerFactory crawlerFactory) : IDataSourceProcessor
{
    private readonly ICrawlerFactory _crawlerFactory = crawlerFactory;

    public async Task ProcessAsync(Tenant tenant)
    {
        foreach (var dataSource in tenant.DataSource)
        {
            var crawler = _crawlerFactory.Create(tenant.Id, dataSource);

            await crawler.CrawAsync();
        }
    }
}
