using CodeReviewAnalyzer.Api.Models.Paging;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Api.Features.People.Models.Responses;

public class PeoplePaginated(
    PageReturn<IEnumerable<PersonLookup>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<PersonLookup>>(
        pageResult,
        pageFilter)
{
}
