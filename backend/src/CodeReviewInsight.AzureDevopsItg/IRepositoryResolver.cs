using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.AzureDevopsItg;

public interface IRepositoryResolver
{
    Task<IEnumerable<GitRepository>> GetRepositoriesFromAsync(string project);
}
