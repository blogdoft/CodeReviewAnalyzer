using CodeReviewAnalyzer.Api.Features.Teams.Models;
using CodeReviewAnalyzer.Api.Models.Paging;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Api.Features.Teams.Models;

public class TeamsPaginated(
    PageReturn<IEnumerable<TeamResponse>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<TeamResponse>>(
        pageResult,
        pageFilter)
{
}
