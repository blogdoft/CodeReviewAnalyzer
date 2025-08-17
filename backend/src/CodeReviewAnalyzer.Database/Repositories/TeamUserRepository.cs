using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.TablesViews;

namespace CodeReviewAnalyzer.Database.Repositories;

public class TeamUserRepository(IDatabaseFacade databaseFacade) : ITeamUser
{
    public const string SelectUsers =
        """
            select t.shared_key as teamId 
                 , tp."role" as role
                 , tp.joined_at_utc as JoinedAtUtc
                 , p.shared_key  as personId
                 , p."name" as PersonName
                 , tn.shared_key as TenantId
            from teams t
                join tenants tn on tn.id = t.tenant_id
                join teams_people tp on tp.team_id = t.id 
                join people p on p.id  = tp.person_id 

        """;

    private const string InsertTeamUser =
        """
            INSERT INTO public.teams_people (
                  tenant_id
                , team_id
                , team_external_id
                , person_id
                , person_external_id
                , "role"
                , joined_at_utc
            ) VALUES (
                  (SELECT id FROM public.tenants WHERE shared_key = @tenantId)
                , (SELECT id FROM public.teams t WHERE t.shared_key = @TeamId)
                , (SELECT t.external_id FROM public.teams t WHERE t.shared_key = @TeamId)
                , (SELECT id FROM public.people p where p.shared_key = @PersonId)
                , (SELECT p.external_id FROM public.people p where p.shared_key = @PersonId)
                , @role
                , @joinedAtUtc
            );            

        """;

    private const string RemoveUserFromTeam =
        """
            delete from "teams_people"
            where team_id = (SELECT id FROM public.teams t WHERE t.shared_key = @TeamId)
              and person_id = (SELECT id FROM public.people p where p.shared_key = @PersonId)
              and tenant_id = (SELECT id FROM public.tenants tn WHERE tn.shared_key = @tenantId);
        
        """;

    public async Task<IEnumerable<TeamPerson>> GetUserFromTeamAsync(Guid tenantId, Guid teamId)
    {
        const string Where =
        """
            where t.shared_key = @sharedKey
              and tn.shared_key = @tenantId

        """;
        const string OrderBy = " order by p.\"name\"";

        var view = await databaseFacade.QueryAsync<TeamPeopleView>(
            SelectUsers + Where + OrderBy,
            new
            {
                sharedKey = teamId,
                tenantId,
            });

        return view.Select(v => v.ExtractTeamPerson());
    }

    public async Task<IEnumerable<TeamPerson>> AddUsersAsync(
        Guid tenantId,
        Guid teamId,
        IEnumerable<TeamPerson> users)
    {
        foreach (var teamUser in users)
        {
            await databaseFacade.ExecuteAsync(
                InsertTeamUser,
                new
                {
                    tenantId,
                    PersonId = teamUser.Person.Id,
                    TeamId = teamId,
                    teamUser.Role,
                    JoinedAtUtc = DateTime.UtcNow,
                });
        }

        return await GetUserFromTeamAsync(tenantId, teamId);
    }

    public async Task<IEnumerable<TeamPerson>> RemoveUserFromAsync(
        Guid tenantId,
        Guid teamId,
        Guid userId)
    {
        await databaseFacade.ExecuteAsync(RemoveUserFromTeam, new
        {
            tenantId,
            TeamId = teamId,
            PersonId = userId,
        });

        return await GetUserFromTeamAsync(tenantId, teamId);
    }
}
