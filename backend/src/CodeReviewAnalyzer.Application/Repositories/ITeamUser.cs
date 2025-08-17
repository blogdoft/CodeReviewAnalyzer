using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface ITeamUser
{
    Task<IEnumerable<TeamPerson>> GetUserFromTeamAsync(
        Guid tenantId,
        Guid teamId);

    Task<IEnumerable<TeamPerson>> AddUsersAsync(
        Guid tenantId,
        Guid teamId,
        IEnumerable<TeamPerson> users);

    Task<IEnumerable<TeamPerson>> RemoveUserFromAsync(
        Guid tenantId,
        Guid teamId,
        Guid userId);
}
