using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface ITeamUser
{
    Task<IEnumerable<TeamUser>> GetUserFromTeamAsync(string teamId);

    Task<IEnumerable<TeamUser>> AddUsersAsync(
        string teamId,
        IEnumerable<TeamUser> users);

    Task<IEnumerable<TeamUser>> RemoveUserFromAsync(string teamId, string userId);
}
