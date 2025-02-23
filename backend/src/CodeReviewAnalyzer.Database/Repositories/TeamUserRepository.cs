using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;
using CodeReviewAnalyzer.Database.TablesViews;

namespace CodeReviewAnalyzer.Database.Repositories;

public class TeamUserRepository(IDatabaseFacade databaseFacade) : ITeamUser
{
    public const string SelectUsers =
        """
            SELECT t.external_id as ExternalId
                 , tu.role_in_team as Role
                 , tu.joined_at_utc as JoinedAtUtc
                 , u."EXTERNAL_IDENTIFIER" as UserId
                 , u."NAME" as UserName
                 , u."ACTIVE" as UserActive
            FROM "TEAMS" t 
                join "TEAM_USER" tu on tu.team_id = t.id
                join "USERS" u on u."ID" = tu.user_id

        """;

    private const string InsertTeamUser =
        """
            INSERT INTO public."TEAM_USER" (
                  team_id
                , user_id
                , role_in_team
                , joined_at_utc
            ) VALUES(
                  (SELECT id FROM public."TEAMS" t where t.external_id = @TeamId)
                , (SELECT "ID" FROM public."USERS"u where u."EXTERNAL_IDENTIFIER" = @UserId)
                , @RoleInTeam
                , @JoinedAtUtc 
            );

        """;

    private const string RemoveUserFromTeam =
        """
            delete from "TEAM_USER"
            where team_id = @TeamId
              and user_id = @UserId;
        
        """;

    public async Task<IEnumerable<TeamUser>> GetUserFromTeamAsync(Guid teamId)
    {
        const string Where =
            "where t.external_id = @ExternalId";
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
        Guid teamId,
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

    public async Task<IEnumerable<TeamUser>> RemoveUserFrom(Guid teamId, Guid userId)
    {
        await databaseFacade.ExecuteAsync(RemoveUserFromTeam, new
        {
            TeamId = teamId,
            UserId = userId,
        });

        return await GetUserFromTeamAsync(teamId);
    }
}
