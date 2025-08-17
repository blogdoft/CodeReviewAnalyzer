using CodeReviewAnalyzer.Api.Models.Paging;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Api.Models.Users;

public class UsersPaginated(
    PageReturn<IEnumerable<Person>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<Person>>(pageResult, pageFilter)
{
}
