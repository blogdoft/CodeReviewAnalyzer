using CodeReviewInsight.Domain.Features.Configurations.Entities;
using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.Application.Services.Crawlers;

public interface IGitRepositoryCrawler
{
    Task<IEnumerable<GitRepository>> GetRepositoriesFromAsync(IEnumerable<DataSource> enumerable);
}
