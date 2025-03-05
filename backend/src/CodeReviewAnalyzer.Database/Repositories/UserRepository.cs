using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Models.PagingModels;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;
using CodeReviewAnalyzer.Database.Extensions;
using CodeReviewAnalyzer.Database.Services;

namespace CodeReviewAnalyzer.Database.Repositories;

public sealed class UserRepository(IDatabaseFacade databaseFacade) : IUsers
{
    private const string UpsertSql =
        """
            INSERT INTO public."USERS" (
                 "EXTERNAL_IDENTIFIER"
                , "NAME"
                , "ACTIVE"
            ) VALUES (
                  @Id
                , @Name
                , @Active
            ) on conflict ("EXTERNAL_IDENTIFIER") do UPDATE SET "NAME"=@Name, "ACTIVE"=@Active;

            
        """;

    private const string ResultSet =
        """
            SELECT u."EXTERNAL_IDENTIFIER" as Id
                 , u."NAME"
                 , u."ACTIVE" 
            FROM public."USERS" u

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
                .AndWith(userName, "u.\"NAME_SH\" like @Name")
                .AndWith(status, "u.\"ACTIVE\" = @Status"))
            .WithPagination(pageFilter)
            .MappingOrderWith("name", "u.\"NAME\"")
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
        await _databaseFacade.ExecuteAsync(UpsertSql, createdBy);
}
