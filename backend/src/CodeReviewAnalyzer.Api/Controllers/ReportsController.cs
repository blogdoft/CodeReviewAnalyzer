using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.Reports;
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

    /// <summary>
    /// Retrieve pull request outliers
    /// </summary>
    /// <remarks>
    /// This endpoint returns pull request records identified as outliers for the
    /// specified metrics.It only considers records where CLOSED_DATE is between the
    /// provided startDate and endDate and teams when specified.
    /// </remarks>
    /// <param name="filter">Period begin.</param>
    /// <response code="200">List of pull request outliers.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("pull-requests:outliers")]
    [ProducesResponseType(typeof(IEnumerable<PullRequestOutlier>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPullRequestOutliersAsync(
            [FromQuery] ReportFilter filter)
    {
        var pullRequestReport = await report.GetPullRequestOutlier(filter);

        return Ok(pullRequestReport);
    }

    /// <summary>
    /// Retrieve pull request rightness index.
    /// </summary>
    /// <remarks>
    /// This index show how preventive the code review are being to mitigate bugs
    /// and defect.
    /// As bugs can be found after a while, this metrics could change as the time goes by.
    /// For this reason, we choose look for a time frame and understand the behavior.
    /// </remarks>
    /// <param name="filter">Period begin.</param>
    /// <response code="200">List of pull request outliers.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("code-review-rightness")]
    [ProducesResponseType(typeof(CodeReviewRightness), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCodeReviewRightnessAsync([FromQuery] ReportFilter filter)
    {
        return Ok();
    }
}
