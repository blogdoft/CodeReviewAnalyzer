using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Services.Processors;

public interface IDataSourceProcessor
{
    Task ProcessAsync(IEnumerable<DataSource> dataSource);
}
