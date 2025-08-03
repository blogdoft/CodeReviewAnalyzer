using BlogDoFT.Libs.DapperUtils.Abstractions;
using BlogDoFT.Libs.DapperUtils.Abstractions.Extensions;
using BlogDoFT.Libs.DapperUtils.Postgres;
using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;
using CodeReviewAnalyzer.Application.Repositories;

namespace CodeReviewAnalyzer.Database.Repositories;

public sealed class PeopleRepository(IDatabaseFacade databaseFacade) : IUsers
{
    private const string UpsertSql =
        """
            INSERT INTO public.people (
                  tenants_id
                , external_id
                , shared_key
                , "name"
                , avatar_url
            ) VALUES (
                , @tenantId
                , @id
                , @sharedKey
                , @name
                , @avatarUrl    
            )
            ON CONFLICT (id)
            DO UPDATE SET  
                  external_id=@id
                , shared_key=@sharedKey
                , "name"=@name
                , avatar_url=@avatarUrl;
            
        """;

    private const string ResultSet =
        """
            SELECT id
                 , p.tenants_id as tenantId
                 , p.external_id as externalId
                 , p.shared_key as sharedKey
                 , p."name"
                 , p.name_sh
                 , p.avatar_url as avatarUrl
            FROM public.people p;

        """;

    private readonly IDatabaseFacade _databaseFacade = databaseFacade;

    public async Task<PageReturn<IEnumerable<User>>> GetAllAsync(
        string? userName,
        bool? status,
        PageFilter pageFilter)
    {
        var (query, pageCount) = new PaginatedSqlBuilder()
            .WithResultSet(ResultSet)
            .WithWhere(whereBuilder => whereBuilder
                .AndWith(userName, "p.\"name_sh\" like @Name"))
            .WithPagination(pageFilter)
            .MappingOrderWith("name", "u.\"name\"")
            .Build();

        var param = new
        {
            Name = userName?.AsSqlWildCard(),
            Status = status,
        };

        var totalItems = await _databaseFacade.QuerySingleOrDefaultAsync<int>(
            pageCount.ToString(),
            param);

        var content = await _databaseFacade.QueryAsync<User>(query.ToString(), param);

        return new PageReturn<IEnumerable<User>>(content, totalItems);
    }

    public async Task Upsert(IntegrationUser createdBy) =>
        await _databaseFacade.ExecuteAsync(UpsertSql, new
        {
            createdBy.Id,
            createdBy.Name,
            NameSh = createdBy.Name.ToUpper(),
            createdBy.Active,
        });
}
