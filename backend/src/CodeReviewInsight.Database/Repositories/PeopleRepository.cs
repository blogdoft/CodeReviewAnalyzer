using BlogDoFT.Libs.DapperUtils.Abstractions;
using BlogDoFT.Libs.DapperUtils.Abstractions.Extensions;
using BlogDoFT.Libs.DapperUtils.Postgres;
using CodeReviewInsight.Application.Integrations.Models;
using CodeReviewInsight.Application.Models.PagingModels;
using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Database.TablesViews;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.People;
using System.Data;
using System.Text.Json;

namespace CodeReviewInsight.Database.Repositories;

public sealed class PeopleRepository(IDatabaseFacade databaseFacade) : IPeople
{
    private const string UpsertSql =
        """
            INSERT INTO public.people (
                  tenant_id
                , external_id
                , shared_key
                , "name"
                , name_sh
                , avatar_url
            ) VALUES (
                  (select id from tenants tn where tn.shared_key = @tenantId)
                , @externalId
                , @sharedKey
                , @name
                , @nameSh
                , @avatarUrl    
            )
            ON CONFLICT (shared_key)
            DO UPDATE SET  
                  external_id=@externalId
                , "name"=@name
                , name_sh=@nameSh
                , avatar_url=@avatarUrl;
            
        """;

    private const string ResultSet =
        """
            SELECT tn.shared_key as tenantId
                 , p.external_id as externalId
                 , p.shared_key as Id
                 , p."name"
                 , p.name_sh
                 , p.avatar_url as avatarUrl
            FROM public.people p
              join tenants tn on tn.id = p.tenant_id

        """;

    private readonly IDatabaseFacade _databaseFacade = databaseFacade;

    public async Task<Person> UpsertPersonAsync(TenantId tenantId, Person person)
    {
        await _databaseFacade.ExecuteAsync(
            UpsertSql,
            new
            {
                TenantId = tenantId.Value,
                ExternalId = person.ExternalId ?? person.Id.ToString(),
                SharedKey = person.Id,
                person.Name,
                NameSh = person.Name.ToSearchable(),
                AvatarUrl = person.AvatarUrl?.ToString(),
            });

        return await GetByIdAsync(tenantId, person.Id) ?? throw new DataException(
            string.Format(
                "Error while storing person {0}",
                JsonSerializer.Serialize(person)));
    }

    public async Task<Person> UpdatePerson(TenantId tenantId, Person person)
    {
        return await UpsertPersonAsync(tenantId, person);
    }

    public async Task<Person?> GetByIdAsync(TenantId tenantId, Guid personId)
    {
        var personFound = await _databaseFacade.QuerySingleOrDefaultAsync<PersonTable>(
            ResultSet + " where p.shared_key = @sharedKey and tn.shared_key = @tenantId",
            new { SharedKey = personId, TenantId = tenantId.Value });
        if (personFound is null)
        {
            return null;
        }

        return personFound.ToEntity();
    }

    public async Task<PageReturn<IEnumerable<Person>>> GetAllAsync(
        Guid tenantId,
        string? personName,
        bool? status,
        PageFilter pageFilter)
    {
        var (query, pageCount) = new PaginatedSqlBuilder()
            .WithResultSet(ResultSet)
            .WithWhere(whereBuilder => whereBuilder
                .AndWith(tenantId, "tn.shared_key = @tenantId")
                .AndWith(personName, "p.\"name_sh\" like @Name"))
            .WithPagination(pageFilter)
            .MappingOrderWith("name", "p.\"name\"")
            .Build();

        var param = new
        {
            tenantId,
            Name = personName?.AsSqlWildCard(),
            Status = status,
        };

        var totalItems = await _databaseFacade.QuerySingleOrDefaultAsync<int>(
            pageCount.ToString(),
            param);

        var content = await _databaseFacade.QueryAsync<Person>(query.ToString(), param);

        return new PageReturn<IEnumerable<Person>>(content, totalItems);
    }

    public async Task UpsertAsync(IntegrationPerson createdBy) =>
        await _databaseFacade.ExecuteAsync(UpsertSql, new
        {
            createdBy.Id,
            createdBy.Name,
            NameSh = createdBy.Name.ToUpper(),
            createdBy.Active,
        });
}
