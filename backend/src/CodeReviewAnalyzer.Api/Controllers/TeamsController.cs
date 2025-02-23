using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Application.Services.Teams;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace CodeReviewAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    /// <param name="teamsRepository">Dependency injection</param>
    /// <param name="team" >Team to be created.</param>
    /// <response code="201">Team created.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Team), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTeamAsync(
        [FromServices] ICreateTeam teamsRepository,
        [FromBody] Team team)
    {
        var response = await teamsRepository.AddAsync(team);

        if (response is null)
        {
            return BadRequest();
        }

        return Created();
    }

    /// <summary>
    /// Return a list of Teams.
    /// </summary>
    /// <param name="teamsRepository">Dependency injection</param>
    /// <param name="teamName">Query team with this name. Use "*" as wildcard. This field is case insensitive.</param>
    /// <returns>A list of Teams found.</returns>
    /// <response code="200">A Team's list.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(Team), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllTeamsAsync(
        [FromServices] ITeams teamsRepository,
        [FromQuery] string? teamName)
    {
        IEnumerable<Team> teamsFound = await teamsRepository.QueryBy(teamName);

        return Ok(teamsFound);
    }

    /// <summary>
    /// Return a specific detailed team data.
    /// </summary>
    /// <param name="id">Team external id.</param>
    /// <returns>Team found.</returns>
    /// <response code="200">Team Found.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTeamByIdAsync([FromRoute][Required] Guid id)
    {
        var teamsFound = await teamsRepository.QueryBy(id);

        return Ok(teamsFound);
    }

    /// <summary>
    /// Deactivate a Team. This endpoint implements a soft-delete.
    /// </summary>
    /// <param name="id">Team external id.</param>
    /// <returns>No content</returns>
    /// <response code="204">Team deleted.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTeamAsync([FromRoute][Required] Guid id)
    {
        await teamsRepository.DeactivateAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Update a Team.
    /// </summary>
    /// <param name="id">Team external id to be updated.</param>
    /// <param name="updateTeam">Data to be overwritten.</param>
    /// <returns>The Team updated.</returns>
    /// <response code="200">Team updated.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTeamAsync(
        [FromRoute][Required] Guid id,
        [FromBody] Team updateTeam)
    {
        if (updateTeam.ExternalId != id)
        {
            return BadRequest();
        }

        await teamsRepository.UpdateAsync(updateTeam);

        return Ok(updateTeam);
    }

    /// <summary>
    /// Return a list of users assigned to a Team.
    /// </summary>
    /// <param name="teamUserRepository">Dependency injection</param>
    /// <param name="teamId">Team external identifier.</param>
    /// <returns>List of Teams' users.</returns>
    /// <response code="200">List of Teams' users</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet("{teamId}/users")]
    [ProducesResponseType(typeof(IEnumerable<TeamUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTeamsUsersAsync(
        [FromServices] ITeamUser teamUserRepository,
        [FromRoute][Required] Guid teamId)
    {
        IEnumerable<TeamUser> teamUsers = await teamUserRepository
            .GetUserFromTeamAsync(teamId);

        return Ok(teamUsers);
    }

    /// <summary>
    /// Add users to a team
    /// </summary>
    /// <param name="teamUserRepository">Dependency injection</param>
    /// <param name="teamId">Team external identifier.</param>
    /// <param name="users">A list of users that should be added to a team.</param>
    /// <returns>Current list of users.</returns>
    /// <response code="200">Users added to a team.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPost("{teamId}/users")]
    [ProducesResponseType(typeof(IEnumerable<TeamUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddUsersAsync(
        [FromServices] ITeamUser teamUserRepository,
        [FromRoute][Required] Guid teamId,
        [FromBody] IEnumerable<TeamUser> users)
    {
        IEnumerable<TeamUser> teamUsers = await teamUserRepository
            .AddUsersAsync(teamId, users);

        return Ok(teamUsers);
    }

    /// <summary>
    /// Remove User from a Team
    /// </summary>
    /// <param name="teamUserRepository">Dependency injection</param>
    /// <param name="teamId">Team external identifier.</param>
    /// <param name="userId">User external id to be removed.</param>
    /// <returns>Current list of users.</returns>
    /// <response code="200">Users added to a team.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpDelete("{teamId}/users/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<TeamUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveBatchUsersAsync(
        [FromServices] ITeamUser teamUserRepository,
        [FromRoute][Required] Guid teamId,
        [FromBody] Guid userId)
    {
        IEnumerable<TeamUser> teamUsers = await teamUserRepository
            .RemoveUserFrom(teamId, userId);

        return Ok(teamUsers);
    }
}
