using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.Application.Services.Crawlers.AzureCrawlers;

public interface IAzureFacade
{
    void SetContext();

    Task<IEnumerable<GitRepository>> GetRepositoriesAsync();
}
