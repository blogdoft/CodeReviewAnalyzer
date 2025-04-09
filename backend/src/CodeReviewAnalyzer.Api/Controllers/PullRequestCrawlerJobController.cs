using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Services;
using CodeReviewAnalyzer.Application.Services.Crawlers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CodeReviewAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class PullRequestCrawlerJobController(PullRequestMetadataProcessor metadataProcessor) : ControllerBase
{
    /// <summary>
    /// Schedule the data load from Azure DevOps
    /// </summary>
    /// <remarks>
    /// Execute, in background, ETL from Azure DevOps to our database
    /// </remarks>
    /// <param name="begin" example="2024-01-01">Period begin.</param>
    /// <param name="end" example="2025-02-28">Period end.</param>
    /// <response code="200">Job scheduled and running</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SchedulePullRequestCrawlingAsync(
        [FromQuery] DateOnly? begin,
        [FromQuery] DateOnly? end)
    {
        await Task.Run(() => metadataProcessor.ExecuteAsync(
            begin: begin ?? new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1),
            end: end ?? DateOnly.FromDateTime(DateTime.Now.AddMonths(1).AddTicks(-1))));
        return Ok();
    }

    /// <summary>
    /// Schedule rework items crawling
    /// </summary>
    /// <remarks>
    /// Query for bugs and defect in an given period and its respective stories.
    /// </remarks>
    /// <param name="crawler">Dependency injection</param>
    /// <param name="begin" example="2025-01-01">Period begin.</param>
    /// <param name="end" example="2025-04-30">Period end.</param>
    /// <response code="200">Job scheduled and running</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("workitems:rework")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ScheduleWorkItemsCrawlingAsync(
        [FromServices] IWorkItemsCrawler crawler,
        [FromQuery] DateOnly? begin,
        [FromQuery] DateOnly? end)
    {
        await crawler.CrawAsync(new ReportFilter
        {
            From = begin ?? new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1),
            To = end ?? DateOnly.FromDateTime(DateTime.Now.AddMonths(1).AddTicks(-1)),
        });

        return Ok();
    }
}
