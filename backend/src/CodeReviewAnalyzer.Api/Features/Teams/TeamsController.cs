using CodeReviewAnalyzer.Api.Features.Teams.Models;
using CodeReviewAnalyzer.Api.Models.Paging;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Application.Services.Teams;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace CodeReviewAnalyzer.Api.Features.Teams;

[ApiController]
[Route("api/{tenantId}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class TeamsController(ITeams teamsRepository) : ControllerBase
{
    /// <summary>
    /// Create a new team.
    /// </summary>
    /// <remarks>
    /// A team is a group of people, designed for analysis.
    /// </remarks>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant identifier</param>
    /// <param name="teamsRepository">Dependency injection</param>
    /// <param name="team" >Team to be created.</param>
    /// <response code="201">Team created.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTeamRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTeamAsync(
        [FromRoute] Guid tenantId,
        [FromServices] ICreateTeam teamsRepository,
        [FromBody] CreateTeamRequest team)
    {
        var response = await teamsRepository.AddAsync(team.ToEntity(tenantId));

        if (response is null)
        {
            return BadRequest();
        }

        return Created();
    }

    /// <summary>
    /// Return a list of Teams.
    /// </summary>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant identifier</param>
    /// <param name="teamsRepository">Dependency injection</param>
    /// <param name="teamName">Query team with this name. Use "*" as wildcard. This field is case insensitive.</param>
    /// <param name="paging">Pagination filter</param>
    /// <returns>A list of Teams found.</returns>
    /// <response code="200">A Team's list.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(TeamsPaginated), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllTeamsAsync(
        [FromRoute] Guid tenantId,
        [FromServices] ITeams teamsRepository,
        [FromQuery] string? teamName,
        [FromQuery] PaginatedRequest paging)
    {
        var teamResult = await teamsRepository.QueryBy(
            paging.ToPageFilter(),
            tenantId,
            teamName);

        var pageResult = PageReturn<IEnumerable<TeamResponse>>.From(
            teamResult,
            () => teamResult.Data.Select(TeamResponse.From));

        var teamResponse = new TeamsPaginated(pageResult, paging);

        return Ok(teamResponse);
    }

    /// <summary>
    /// Return a specific detailed team data.
    /// </summary>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant id.</param>
    /// <param name="teamId" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Team external id.</param>
    /// <returns>Team found.</returns>
    /// <response code="200">Team Found.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("{teamId}")]
    [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamByIdAsync(
        [FromRoute][Required] Guid tenantId,
        [FromRoute][Required] Guid teamId)
    {
        var teamFound = await teamsRepository.QueryByIdAsync(tenantId, teamId);

        if (teamFound is null)
        {
            return NotFound();
        }

        var response = TeamResponse.From(teamFound);

        return Ok(response);
    }

    /// <summary>
    /// Deactivate a Team. This endpoint implements a soft-delete.
    /// </summary>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant id.</param>
    /// <param name="teamId" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Team external id.</param>
    /// <returns>No content</returns>
    /// <response code="204">Team deleted.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpDelete("{teamId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTeamAsync(
        [FromRoute][Required] Guid tenantId,
        [FromRoute][Required] Guid teamId)
    {
        await teamsRepository.DeactivateAsync(tenantId, teamId);

        return NoContent();
    }

    /// <summary>
    /// Update a Team.
    /// </summary>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant id</param>
    /// <param name="teamId"  example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Team external id to be updated.</param>
    /// <param name="updateTeam">Data to be overwritten.</param>
    /// <returns>The Team updated.</returns>
    /// <response code="200">Team updated.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPut("{teamId}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTeamAsync(
        [FromRoute][Required] Guid tenantId,
        [FromRoute][Required] Guid teamId,
        [FromBody] UpdateTeamRequest updateTeam)
    {
        var teamEntity = updateTeam.ToEntity(tenantId, teamId);

        await teamsRepository.UpdateAsync(teamEntity);

        return Ok(updateTeam);
    }

    /// <summary>
    /// Return a list of users assigned to a Team.
    /// </summary>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant id.</param>
    /// <param name="teamUserRepository">Dependency injection</param>
    /// <param name="teamId" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Team external identifier.</param>
    /// <returns>List of Teams' users.</returns>
    /// <response code="200">List of Teams' users</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("{teamId}/users")]
    [ProducesResponseType(typeof(IEnumerable<TeamPerson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamsUsersAsync(
        [FromRoute][Required] Guid tenantId,
        [FromServices] ITeamUser teamUserRepository,
        [FromRoute][Required] Guid teamId)
    {
        IEnumerable<TeamPerson> teamUsers = await teamUserRepository
            .GetUserFromTeamAsync(tenantId, teamId);

        return Ok(teamUsers);
    }

    /// <summary>
    /// Add users to a team
    /// </summary>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant id.</param>
    /// <param name="teamUserRepository">Dependency injection</param>
    /// <param name="teamId" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Team external identifier.</param>
    /// <param name="users">A list of users that should be added to a team.</param>
    /// <returns>Current list of users.</returns>
    /// <response code="200">Users added to a team.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPost("{teamId}/users")]
    [ProducesResponseType(typeof(IEnumerable<TeamPerson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUsersAsync(
        [FromRoute][Required] Guid tenantId,
        [FromServices] ITeamUser teamUserRepository,
        [FromRoute][Required] Guid teamId,
        [FromBody] IEnumerable<UpdateTeamPerson> users)
    {
        var teamUsers = users.Select(utp => utp.ToEntity(tenantId));

        IEnumerable<TeamPerson> added = await teamUserRepository
            .AddUsersAsync(tenantId, teamId, teamUsers);

        return Ok(added);
    }

    /// <summary>
    /// Remove User from a Team
    /// </summary>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant id.</param>
    /// <param name="teamUserRepository">Dependency injection</param>
    /// <param name="teamId" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Team external identifier.</param>
    /// <param name="userId">User external id to be removed.</param>
    /// <returns>Current list of users.</returns>
    /// <response code="204">User removed from team.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpDelete("{teamId}/users/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveBatchUsersAsync(
        [FromRoute][Required] Guid tenantId,
        [FromServices] ITeamUser teamUserRepository,
        [FromRoute][Required] Guid teamId,
        [FromRoute] Guid userId)
    {
        await teamUserRepository
            .RemoveUserFromAsync(tenantId, teamId, userId);

        return NoContent();
    }
}
