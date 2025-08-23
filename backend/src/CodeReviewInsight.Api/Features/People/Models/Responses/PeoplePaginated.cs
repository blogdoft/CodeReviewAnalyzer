using CodeReviewInsight.Api.Models.Paging;
using CodeReviewInsight.Application.Models.PagingModels;

namespace CodeReviewInsight.Api.Features.People.Models.Responses;

public class PeoplePaginated(
    PageReturn<IEnumerable<PersonLookup>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<PersonLookup>>(
        pageResult,
        pageFilter)
{
}
