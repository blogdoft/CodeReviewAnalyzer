using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Database.TablesViews;

public class TeamUserView
{
    public Guid TeamId { get; set; }

    public string? Role { get; set; }

    public DateTime JoinedAtUtc { get; set; }

    public string? UserName { get; set; }

    public string? UserId { get; set; }

    public bool UserActive { get; set; }

    internal TeamUser ExtractTeamUser() => new()
    {
        User = ExtractUser(),
        Role = Role ?? "Unknown",
    };

    private User ExtractUser() => new()
    {
        Id = UserId ?? Guid.Empty.ToString(),
        Name = UserName ?? "This user could be deleted early.",
        Active = UserActive,
    };
}
