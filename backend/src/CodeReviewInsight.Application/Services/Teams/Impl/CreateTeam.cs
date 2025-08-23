using CodeReviewInsight.Application.Models;
using CodeReviewInsight.Application.Repositories;

namespace CodeReviewInsight.Application.Services.Teams.Impl;

internal class CreateTeam(ITeams teams) : ICreateTeam
{
    public async Task<Team> AddAsync(Team team)
    {
        return await teams.AddAsync(team);
    }
}
