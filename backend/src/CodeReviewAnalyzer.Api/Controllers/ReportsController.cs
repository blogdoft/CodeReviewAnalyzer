using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Application.Models.PullRequestReport;
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
    /// <param name="begin" example="2024-01-01">Period begin.</param>
    /// <param name="end" example="2025-02-28">Period end.</param>
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
        [FromQuery] DateOnly? begin,
        [FromQuery] DateOnly? end)
    {
        var pullRequestReport = await report.GetPullRequestTimeReportAsync(
            begin ?? new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1),
            end ?? DateOnly.FromDateTime(DateTime.Now.AddMonths(1).AddTicks(-1)));

        return Ok(pullRequestReport);
    }

    /// <summary>
    /// ...
    /// </summary>
    /// <remarks>
    /// ..
    /// </remarks>
    /// <param name="begin" example="2024-01-01">Period begin.</param>
    /// <param name="end" example="2025-02-28">Period end.</param>
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
        [FromQuery] DateOnly? begin,
        [FromQuery] DateOnly? end)
    {
        var reviewerDensity = await report.GetUserReviewerDensity(
            begin ?? new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1),
            end ?? DateOnly.FromDateTime(DateTime.Now.AddMonths(1).AddTicks(-1)));

        return Ok(reviewerDensity);
    }
}
