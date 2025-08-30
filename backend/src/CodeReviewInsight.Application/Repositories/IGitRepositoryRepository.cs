using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.Application.Repositories;

public interface IGitRepositoryRepository
{
    Task BulkUpsertAsync(IEnumerable<GitRepository> extractedRepositories);
}
