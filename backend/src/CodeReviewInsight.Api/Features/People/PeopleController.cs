using CodeReviewInsight.Api.Features.People.Models.Requests;
using CodeReviewInsight.Api.Features.People.Models.Responses;
using CodeReviewInsight.Api.Models.Paging;
using CodeReviewInsight.Application.Models.PagingModels;
using CodeReviewInsight.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CodeReviewInsight.Api.Features.People;

[ApiController]
[Route("api/{tenantId}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class PeopleController : ControllerBase
{
    /// <summary>
    /// Create a new team.
    /// </summary>
    /// <remarks>
    /// A team is a group of people, designed for analysis.
    /// </remarks>
    /// <param name="people">Service Injection</param>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant identifier</param>
    /// <param name="personName" example="John D*">Person name. Use wild card (*).</param>
    /// <param name="status">Person status.
    /// <ul>
    ///     <li>Active</li>
    ///     <li>Dactive</li>
    /// </ul>
    /// </param>
    /// <param name="pageFilter">Pagination filter.</param>
    /// <response code="201">Team created.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpGet]
    [ProducesResponseType<PeoplePaginated>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] IPeople people,
        [FromRoute] Guid tenantId,
        [FromQuery] string? personName,
        [FromQuery] bool? status,
        [FromQuery] PaginatedRequest pageFilter)
    {
        var personResult = await people.GetAllAsync(
            tenantId,
            personName,
            status,
            pageFilter.ToPageFilter());

        var pageResult = PageReturn<IEnumerable<PersonLookup>>.From(
            personResult,
            () => personResult.Data.Select(PersonLookup.From));

        var userResponse = new PeoplePaginated(
            pageResult,
            pageFilter);

        return Ok(userResponse);
    }

    /// <summary>
    /// Create a new Person.
    /// </summary>
    /// <remarks>
    /// A person is anybody that could interact as user or developer
    /// or even a coordinator.
    /// </remarks>
    /// <param name="people">Service Injection</param>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant identifier</param>
    /// <param name="person">Person to be Registered.</param>
    /// <response code="201">Person registered.</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync(
        [FromServices] IPeople people,
        [FromRoute] Guid tenantId,
        [FromBody] PersonRequest person)
    {
        var personEntity = person.ToEntity(Guid.NewGuid());

        var updatedPerson = await people.UpsertPersonAsync(
            tenantId,
            personEntity);

        var resourceUrl = Url.Action(nameof(GetByIdAsync), new { tenantId, updatedPerson.Id });

        return Created(resourceUrl, new
        {
            tenantId,
            personId = updatedPerson.Id,
        });
    }

    /// <summary>
    /// Return detailed Person's data.
    /// </summary>
    /// <param name="people">Repository</param>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant unique identifier.</param>
    /// <param name="personId" example="da17b5b0-39ab-4b15-9589-fc25e9747ad0">Person unique identifier.</param>
    /// <returns>Detailed person data.</returns>
    [HttpGet("{personId}")]
    [ProducesResponseType<PersonResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> GetByIdAsync(
        [FromServices] IPeople people,
        [FromRoute] Guid tenantId,
        [FromRoute] Guid personId)
    {
        var person = await people.GetByIdAsync(tenantId, personId);

        if (person is null)
        {
            return NotFound();
        }

        return Ok(PersonResponse.From(person));
    }

    /// <summary>
    /// Overwirte person data.
    /// </summary>
    /// <remarks>
    /// All data provided into this endpoint will overwrite current information.
    /// </remarks>
    /// <param name="people">Service Injection</param>
    /// <param name="tenantId" example="42681c98-67b3-4db8-b670-8a413590ff63">Tenant identifier</param>
    /// <param name="personId" example="da17b5b0-39ab-4b15-9589-fc25e9747ad0">Person unique identifier.</param>
    /// <param name="person">Person to be Registered.</param>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server error</response>
    [HttpPut("{personId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(
        [FromServices] IPeople people,
        [FromRoute] Guid tenantId,
        [FromRoute] Guid personId,
        [FromBody] PersonRequest person)
    {
        var personEntity = person.ToEntity(personId);

        var updatedPerson = await people.UpsertPersonAsync(
            tenantId,
            personEntity);

        return Ok(PersonResponse.From(updatedPerson));
    }
}
