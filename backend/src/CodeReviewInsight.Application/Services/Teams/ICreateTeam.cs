using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Services.Teams;

public interface ICreateTeam
{
    Task<Team> AddAsync(Team team);
}
