using CodeReviewInsight.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeReviewInsight.Api.Features.Teams.Models;

public class UpdateTeamPerson
{
    public required Guid PersonId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string? Role { get; set; }

    public TeamPerson ToEntity(Guid tenantId)
    {
        return new TeamPerson()
        {
            Tenant = Tenant.CreateAsLookup(tenantId),
            Person = Person.CreateAsLookup(PersonId),
            Role = Role ?? "Unknown",
        };
    }
}
