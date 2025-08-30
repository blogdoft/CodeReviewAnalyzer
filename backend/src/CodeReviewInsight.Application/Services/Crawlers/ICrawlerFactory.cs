using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Services.Crawlers;

public interface ICrawlerFactory
{
    public IDataSourceCrawler Create(TenantId tenantId, DataSource dataSource);
}
