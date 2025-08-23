using CodeReviewInsight.Api.Models.Paging;
using CodeReviewInsight.Application.Models.PagingModels;

namespace CodeReviewInsight.Api.Features.Teams.Models;

public class TeamsPaginated(
    PageReturn<IEnumerable<TeamResponse>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<TeamResponse>>(
        pageResult,
        pageFilter)
{
}
