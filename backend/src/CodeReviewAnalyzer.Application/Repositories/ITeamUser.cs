using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface ITeamUser
{
    Task<IEnumerable<TeamUser>> GetUserFromTeamAsync(Guid teamId);

    Task<IEnumerable<TeamUser>> AddUsersAsync(
        Guid teamId,
        IEnumerable<TeamUser> users);

    Task<IEnumerable<TeamUser>> RemoveUserFrom(Guid teamId, Guid userId);
}
