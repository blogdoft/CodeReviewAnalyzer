using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface ITeams
{
    Task<Team> AddAsync(Team team);

    Task DeactivateAsync(Guid tenantId, Guid id);

    Task<PageReturn<IEnumerable<Team>>> QueryBy(
        PageFilter pageFilter,
        Guid tenantId,
        string? teamName);

    Task<Team?> QueryByIdAsync(Guid tenantId, Guid id);

    Task UpdateAsync(Team updateTeam);
}
