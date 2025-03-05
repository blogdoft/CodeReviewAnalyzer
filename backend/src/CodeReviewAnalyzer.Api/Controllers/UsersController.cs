using CodeReviewAnalyzer.Api.Models.Paging;
using CodeReviewAnalyzer.Api.Models.Users;
using CodeReviewAnalyzer.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CodeReviewAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class UsersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] IUsers users,
        [FromQuery] string? userName,
        [FromQuery] bool? status,
        [FromQuery] PaginatedRequest pageFilter)
    {
        var userResult = await users.GetAllAsync(userName, status, pageFilter.ToPageFilter());

        var userResponse = new UsersPaginated(
            userResult,
            pageFilter);

        return Ok(userResponse);
    }
}
