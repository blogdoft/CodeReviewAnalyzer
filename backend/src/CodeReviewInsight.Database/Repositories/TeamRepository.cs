using BlogDoFT.Libs.DapperUtils.Abstractions;
using BlogDoFT.Libs.DapperUtils.Abstractions.Extensions;
using BlogDoFT.Libs.DapperUtils.Postgres;
using CodeReviewInsight.Application.Models;
using CodeReviewInsight.Application.Models.PagingModels;
using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Database.TablesViews;

namespace CodeReviewInsight.Database.Repositories;

public class TeamRepository(IDatabaseFacade databaseFacade) : ITeams
{
    private const string Insert =
        """
            INSERT INTO public.teams(
                  tenant_id
                , shared_key
                , external_id
                , "name"
                , name_sh
                , description
            ) VALUES (
                  (select id from tenants where tenants.shared_key = @tenantId )
                , @sharedKey
                , @externalId
                , @name
                , @nameSh
                , @description
            );                

        """;

    private const string Update =
        """
            UPDATE public.teams SET 
                  "name" = @Name
                , name_sh = @NameSh
                , description = @Description
                , active = @Active 
            WHERE shared_key = @sharedKey;

        """;

    private const string TeamResultSet =
        """
            SELECT t.id
                 , tn.shared_key as TenantId
                 , tn."name" as TenantName
                 , t.shared_key as SharedKey
                 , t.external_id as ExternalId
                 , t."name"
                 , t.description
            FROM public.teams t  
                join tenants tn on tn.id  = t.tenant_id      

        """;

    public async Task<Team> AddAsync(Team team)
    {
        await databaseFacade.ExecuteAsync(
            Insert,
            new
            {
                TenantId = team.Tenant.Id,
                team.ExternalId,
                team.SharedKey,
                team.Name,
                NameSh = team.Name.ToUpperInvariant(),
                team.Description,
            });

        return team;
    }

    public async Task DeactivateAsync(Guid tenantId, Guid id)
    {
        const string Sql = """
            update teams set 
                active = false 
            where shared_key = @id 
              and tenant_id = (select tn.id from tenants tn where tn.shared_key = @tenantId)

        """;

        await databaseFacade.ExecuteAsync(Sql, new { id, tenantId });
    }

    public async Task<PageReturn<IEnumerable<Team>>> QueryBy(
        PageFilter pageFilter,
        Guid tenantId,
        string? teamName)
    {
        var (query, pageCount) = new PaginatedSqlBuilder()
            .WithResultSet(TeamResultSet)
            .WithWhere(whereBuilder => whereBuilder
                .AndWith(tenantId, "tn.shared_key = @tenantId")
                .AndWith(teamName, "t.name_sh like @name"))
            .WithPagination(pageFilter)
            .MappingOrderWith("name", "t.name")
            .Build();
        var param = new
        {
            Name = teamName?.AsSqlWildCard(),
            TenantId = tenantId,
        };

        var totalItems = await databaseFacade.QuerySingleOrDefaultAsync<int>(
            pageCount.ToString(),
            param);

        var content = await databaseFacade.QueryAsync<Team>(
            query.ToString(),
            param);

        return new PageReturn<IEnumerable<Team>>(content, totalItems);
    }

    public async Task<Team?> QueryByIdAsync(Guid tenantId, Guid id)
    {
        const string Where = "where t.shared_key = @id and tn.shared_key =  @tenantId";
        var table = await databaseFacade.QuerySingleOrDefaultAsync<TeamsTable>(
            TeamResultSet + Where,
            new { id, tenantId });

        if (table is null)
        {
            return null;
        }

        return new Team()
        {
            Active = table.Active,
            Description = table.Description,
            ExternalId = table.ExternalId,
            Name = table.Name,
            SharedKey = table.SharedKey,
            Tenant = new Tenant()
            {
                Id = table.TenantId,
                Name = table.TenantName,
            },
        };
    }

    public async Task UpdateAsync(Team updateTeam)
    {
        await databaseFacade.ExecuteAsync(Update, new
        {
            updateTeam.SharedKey,
            updateTeam.Name,
            NameSh = updateTeam.Name.AsSqlWildCard(),
            updateTeam.Description,
            updateTeam.Active,
            updateTeam.ExternalId,
        });
    }
}
