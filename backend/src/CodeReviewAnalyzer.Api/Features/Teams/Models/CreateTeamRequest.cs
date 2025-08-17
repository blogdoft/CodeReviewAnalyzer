using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Api.Features.Teams.Models;

public class CreateTeamRequest
{
    public string? ExternalId { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public bool Active { get; init; } = true;

    public Team ToEntity(Guid tenantId) => new()
    {
        Tenant = new Tenant()
        {
            Id = tenantId,
        },
        SharedKey = Guid.NewGuid(),
        ExternalId = ExternalId,
        Name = Name,
        Description = Description,
        Active = Active,
    };
}
