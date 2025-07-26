using CodeReviewAnalyzer.Api.Features.ApplicationSetup.Models.Requests;
using CodeReviewAnalyzer.Api.Features.ApplicationSetup.Responses;
using CodeReviewAnalyzer.Application.TenantFeature;
using CodeReviewAnalyzer.Application.TenantFeature.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace CodeReviewAnalyzer.Api.Features.ApplicationSetup;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ApplicationSetupController : ControllerBase
{
    private readonly ITenantAdd _addTenant;
    private readonly ITenantUpdate _updateTenant;
    private readonly ITenantRepository _repository;

    public ApplicationSetupController(
        ITenantAdd addTenant,
        ITenantUpdate updateTenant,
        ITenantRepository repository)
    {
        _addTenant = addTenant;
        _updateTenant = updateTenant;
        _repository = repository;
    }

    /// <summary>
    /// <para>Create a new tenant.</para>
    /// <para>A tenant represent a company that want to use
    /// this system to collect insights about development process.</para>
    /// </summary>
    /// <param name="request">Request object</param>
    /// <returns>Returns 201 Create with Location header</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTenantAsync(CreateTenantRequest request)
    {
        var tenantId = await _addTenant.Execute(request.To());

        var resourceUrl = Url.Action(nameof(GetByIdAsync), new { id = tenantId });

        return Created(resourceUrl!, new
        {
            Id = tenantId,
        });
    }

    /// <summary>
    /// Return a specific Tenant details data.
    /// </summary>
    /// <param name="tenantId">Tenant Unique identifier</param>
    /// <returns>A tenant</returns>
    [HttpGet("{tenantId}")]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid tenantId)
    {
        var tenant = await _repository.GetByIdAsync(tenantId);
        return Ok(TenantResponse.From(tenant));
    }

    /// <summary>
    /// Updates the setup information of an existing tenant.
    /// </summary>
    /// <remarks>
    /// <para>This endpoint is used to update the configuration of an existing tenant identified by its GUID.</para>
    /// <para><b>Usage:</b></para>
    /// <para>PUT /api/tenants/{tenantId}</para>
    /// <para><b>Request body example:</b></para>
    /// <code>
    /// {
    ///   "name": "FT Solutions",
    ///   "active": true
    /// }
    /// </code>
    /// 
    /// <para><b>Response body example (200 OK):</b></para>
    /// <code>
    /// {
    ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///   "name": "FT Solutions",
    ///   "active": true
    /// }
    /// </code>
    /// <para><b>Notes:</b></para>
    /// <list type="bullet">
    ///   <item><description>The tenantId must be a valid GUID.</description></item>
    ///   <item><description>If the tenant is not found, the server returns 404 Not Found.</description></item>
    ///   <item><description>If the request body is invalid, the server returns 400 Bad Request.</description></item>
    /// </list>
    /// </remarks>
    /// <param name="tenantId">The unique identifier of the tenant to be updated.</param>
    /// <param name="request">The updated tenant information.</param>
    /// <returns>
    /// Returns 200 OK with the updated tenant data if successful;
    /// 400 Bad Request if the input is invalid;
    /// 404 Not Found if the tenant does not exist.
    /// </returns>
    /// <response code="200">Tenant successfully updated</response>
    /// <response code="400">Invalid request payload</response>
    /// <response code="404">Tenant not found</response>
    [HttpPut("{tenantId}")]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTenantSetupAsync(
        [FromRoute][Required] Guid tenantId,
        [FromBody] UpdateTenantRequest request)
    {
        var tenantEntity = request.To(tenantId);
        var updatedTenant = await _updateTenant.ExecuteAsync(tenantEntity);

        return Ok(TenantResponse.From(updatedTenant));
    }
}
