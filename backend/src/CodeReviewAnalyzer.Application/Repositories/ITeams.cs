using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface ITeams
{
    Task<Team> AddAsync(Team team);

    Task DeactivateAsync(Guid id);

    Task<PageReturn<IEnumerable<Team>>> QueryBy(
        PageFilter pageFilter,
        string? teamName);

    Task<Team?> QueryByIdAsync(Guid id);

    Task UpdateAsync(Team updateTeam);
}
