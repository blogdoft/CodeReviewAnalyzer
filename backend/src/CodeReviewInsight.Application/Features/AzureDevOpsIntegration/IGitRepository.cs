using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.Application.Features.AzureDevOpsIntegration;

public interface IGitRepository
{
    Task UpsertRepositoriesAsync(IEnumerable<GitRepository> repositories);
}
