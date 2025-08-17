using CodeReviewAnalyzer.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeReviewAnalyzer.Api.Features.Teams.Models;

public class UpdateTeamRequest
{
    public string? ExternalId { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string Name { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string Description { get; init; }

    public bool Active { get; init; } = true;

    public Team ToEntity(Guid tenantId, Guid id)
    {
        return new Team()
        {
            SharedKey = id,
            ExternalId = ExternalId,
            Name = Name,
            Description = Description,
            Tenant = new Tenant() { Id = tenantId },
            Active = Active,
        };
    }
}
