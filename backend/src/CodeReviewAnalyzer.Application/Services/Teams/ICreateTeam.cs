using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Services.Teams;

public interface ICreateTeam
{
    Task<Team> AddAsync(Team team);
}
