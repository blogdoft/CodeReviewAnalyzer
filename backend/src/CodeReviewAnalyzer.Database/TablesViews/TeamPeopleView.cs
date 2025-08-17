using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Database.TablesViews;

public class TeamPeopleView
{
    public Guid TenantId { get; set; }

    public Guid TeamId { get; set; }

    public string? Role { get; set; }

    public DateTime JoinedAtUtc { get; set; }

    public string? PersonName { get; set; }

    public Guid PersonId { get; set; }

    internal TeamPerson ExtractTeamPerson() => new()
    {
        Tenant = Tenant.CreateAsLookup(TenantId),
        Person = ExtractPerson(),
        Role = Role ?? "Unknown",
    };

    private Person ExtractPerson() => new()
    {
        Id = PersonId,
        Name = PersonName ?? "Maybe this user was deleted early.",
    };
}
