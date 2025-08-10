using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.TablesViews;

namespace CodeReviewAnalyzer.Database.Repositories;

public class TeamUserRepository(IDatabaseFacade databaseFacade) : ITeamUser
{
    public const string SelectUsers =
        """
            select t.shared_key as SharedKey 
                , tp."role" as role
                , tp.joined_at_utc as JoinedAtUtc
                , p.shared_key  as UserId
                , p."name" as UserName
            from teams t
                join teams_people tp on tp.teams_id = t.id 
                join people p on p.id  = tp.people_id 

        """;

    private const string InsertTeamUser =
        """
            INSERT INTO public.teams_people (
                  tenants_id
                , teams_id
                , teams_external_id
                , people_id
                , people_external_id
                , "role"
                , joined_at_utc
            ) VALUES (
                  @tenant_id
                , (SELECT id FROM public.teams t WHERE t.shared_key = @TeamId)
                , @teamsExternalId
                , (SELECT id FROM public.people p where p.shared_key = @PeopleId)
                , @people_external_id
                , @role
                , @joinedAtUtc
            );            

        """;

    private const string RemoveUserFromTeam =
        """
            delete from "teams_people"
            where teams_id = (SELECT id FROM public.teams t WHERE t.shared_key = @TeamId)
              and people_id = (SELECT id FROM public.people p where p.shared_key = @PeopleId);
        
        """;

    public async Task<IEnumerable<TeamUser>> GetUserFromTeamAsync(string teamId)
    {
        const string Where =
            "where t.shared_key = @ExternalId";
        const string OrderBy = " order by u.\"NAME\"";

        var view = await databaseFacade.QueryAsync<TeamUserView>(
            SelectUsers + Where + OrderBy,
            new
            {
                ExternalId = teamId,
            });

        return view.Select(v => v.ExtractTeamUser());
    }

    public async Task<IEnumerable<TeamUser>> AddUsersAsync(
        string teamId,
        IEnumerable<TeamUser> users)
    {
        foreach (var teamUser in users)
        {
            await databaseFacade.ExecuteAsync(
                InsertTeamUser,
                new
                {
                    UserId = teamUser.User.Id,
                    TeamId = teamId,
                    RoleInTeam = teamUser.Role,
                    JoinedAtUtc = DateTime.UtcNow,
                });
        }

        return await GetUserFromTeamAsync(teamId);
    }

    public async Task<IEnumerable<TeamUser>> RemoveUserFromAsync(
        string teamId,
        string userId)
    {
        await databaseFacade.ExecuteAsync(RemoveUserFromTeam, new
        {
            TeamId = teamId,
            UserId = userId,
        });

        return await GetUserFromTeamAsync(teamId);
    }
}
