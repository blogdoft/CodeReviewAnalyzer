using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;

namespace CodeReviewAnalyzer.Application.Services.Teams.Impl;

internal class CreateTeam(ITeams teams) : ICreateTeam
{
    public async Task<Team> AddAsync(Team team)
    {
        return await teams.AddAsync(team);
    }
}
