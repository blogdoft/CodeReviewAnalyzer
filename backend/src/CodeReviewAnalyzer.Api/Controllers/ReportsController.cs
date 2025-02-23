using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CodeReviewAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ReportsController(IReport report) : ControllerBase
{
    /// <summary>
    /// Returns data about Pull request.
    /// </summary>
    /// <remarks>
    /// Based on Closed Pull Request history, this endpoint returns mesures
    /// about Time to Approval, to merge, to receive first comment and etc.
    /// </remarks>
    /// <param name="filter">Period begin.</param>
    /// <response code="200">Pull Request mesures.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("pull-requests")]
    [ProducesResponseType(typeof(PullRequestTimeReport), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPullRequestTimeReportAsync(
        [FromQuery] ReportFilter filter)
    {
        var pullRequestReport = await report.GetPullRequestTimeReportAsync(filter);

        return Ok(pullRequestReport);
    }

    /// <summary>
    /// ...
    /// </summary>
    /// <remarks>
    /// ..
    /// </remarks>
    /// <param name="filter">Filters</param>
    /// <response code="200">Job scheduled and running</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("density")]
    [ProducesResponseType(typeof(PullRequestTimeReport), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserReviewDensityAsync(
        [FromQuery] ReportFilter filter)
    {
        var reviewerDensity = await report.GetUserReviewerDensity(filter);

        return Ok(reviewerDensity);
    }
}
