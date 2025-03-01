using CodeReviewAnalyzer.Api.Models.Paging;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Api.Models.Teams;

public class TeamsPaginated(
    PageReturn<IEnumerable<Team>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<Team>>(
        pageResult,
        pageFilter)
{
}
