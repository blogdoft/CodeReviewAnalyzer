using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;
using CodeReviewAnalyzer.Database.Extensions;

namespace CodeReviewAnalyzer.Database.Repositories;

public class TeamRepository(IDatabaseFacade databaseFacade) : ITeams
{
    private const string Insert =
        """
            INSERT INTO public."TEAMS" (
                  external_id
                , "name"
                , name_sh
                , description
                , active
            ) VALUES (
                  @ExternalId
                , @Name
                , @NameSh
                , @Description
                , @Active);

        """;

    private const string Update =
        """
            UPDATE public."TEAMS" SET 
                  "name" = @Name
                , name_sh = @NameSh
                , description = @Description
                , active = @Active 
            WHERE external_id = @ExternalId;

        """;

    private const string TeamResultSet =
       """
            SELECT t.external_id as ExternalId
                 , t."name" as Name
                 , t.description as Description
                 , t.active as Active
            FROM public."TEAMS" t

       """;

    public async Task<Team> AddAsync(Team team)
    {
        await databaseFacade.ExecuteAsync(
            Insert,
            new
            {
                ExternalId = team.ExternalId,
                Name = team.Name,
                NameSh = team.Name.ToUpperInvariant(),
                Description = team.Description,
                Active = team.Active,
            });

        return team;
    }

    public async Task DeactivateAsync(Guid id)
    {
        const string Sql = """
          update "TEAMS" set active = false where external_id = @id
        """;

        await databaseFacade.ExecuteAsync(Sql, new { id = id.ToString() });
    }

    public async Task<IEnumerable<Team>> QueryBy(string? teamName)
    {
        var where = string.Empty;
        if (!string.IsNullOrWhiteSpace(teamName))
        {
            where = "where t.name_sh like @Name";
        }

        var sql = TeamResultSet + where;

        return await databaseFacade
            .QueryAsync<Team>(sql, new
            {
                Name = (teamName + '%').AsSqlWildCard(),
            });
    }

    public async Task<Team?> QueryByIdAsync(string id)
    {
        const string Where = "where t.external_id = @id";
        return await databaseFacade.QuerySingleOrDefaultAsync<Team>(
            TeamResultSet + Where,
            new { id });
    }

    public async Task UpdateAsync(Team updateTeam)
    {
        await databaseFacade.ExecuteAsync(Update, new
        {
            updateTeam.Name,
            NameSh = updateTeam.Name.AsSqlWildCard(),
            updateTeam.Description,
            updateTeam.Active,
            updateTeam.ExternalId,
        });
    }
}
