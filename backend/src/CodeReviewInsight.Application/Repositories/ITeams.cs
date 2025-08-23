using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Application.Models;
using CodeReviewInsight.Application.Models.PagingModels;

namespace CodeReviewInsight.Application.Repositories;

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
