using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface ITeams
{
    Task<Team> AddAsync(Team team);

    Task DeactivateAsync(Guid id);

    Task<IEnumerable<Team>> QueryBy(string? teamName);

    Task<Team?> QueryByIdAsync(string id);

    Task UpdateAsync(Team updateTeam);
}
