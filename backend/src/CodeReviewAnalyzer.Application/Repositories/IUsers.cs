using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface IUsers
{
    Task Upsert(IntegrationUser createdBy);

    Task<PageReturn<IEnumerable<User>>> GetAllAsync(
        string? userName,
        bool? status,
        PageFilter pageFilter);
}
