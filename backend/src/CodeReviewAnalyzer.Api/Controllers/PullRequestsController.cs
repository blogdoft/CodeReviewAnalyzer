using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace CodeReviewAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class PullRequestsController : ControllerBase
{
    /// <summary>
    /// Returns a single pull requests stats for short analysis.
    /// </summary>
    /// <param name="pullRequests">Repository</param>
    /// <param name="externalId">Pull Request identifier</param>
    /// <returns>Pull request stats.</returns>
    /// <response code="200">The Pull Requests stats.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("{externalId}:stats")]
    [ProducesResponseType(typeof(PullRequestStats), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPullRequestStatsAsync(
        [FromServices] IPullRequests pullRequests,
        [FromRoute][Required] string externalId)
    {
        var stats = await pullRequests.GetStatsFromAsync(externalId);
        if (stats is null)
        {
            return NotFound();
        }

        return Ok(stats);
    }
}
