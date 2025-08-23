using CodeReviewInsight.Api.Models.Paging;
using CodeReviewInsight.Application.Models;
using CodeReviewInsight.Application.Models.PagingModels;

namespace CodeReviewInsight.Api.Models.Users;

public class UsersPaginated(
    PageReturn<IEnumerable<Person>> pageResult,
    PaginatedRequest pageFilter)
    : PaginatedResponse<IEnumerable<Person>>(pageResult, pageFilter)
{
}
