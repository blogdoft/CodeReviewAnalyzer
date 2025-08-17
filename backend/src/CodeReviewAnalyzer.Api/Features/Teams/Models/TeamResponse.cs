using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Api.Features.Teams.Models;

public class TeamResponse
{
    public Guid Id { get; init; }

    public string? ExternalId { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public bool Active { get; init; } = true;

    public static TeamResponse From(Team team) => new()
    {
        Id = team.SharedKey,
        Name = team.Name,
        Description = team.Description,
        ExternalId = team.ExternalId,
    };
}
