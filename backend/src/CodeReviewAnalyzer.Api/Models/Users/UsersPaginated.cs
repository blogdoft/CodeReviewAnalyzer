using CodeReviewAnalyzer.Api.Models.Paging;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Api.Models.Users;

public class UsersPaginated(
    PageReturn<IEnumerable<User>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<User>>(pageResult, pageFilter)
{
}
