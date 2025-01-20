using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;

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

    private readonly IDatabaseFacade _databaseFacade = databaseFacade;

    public async Task Upsert(User createdBy) =>
        await _databaseFacade.ExecuteAsync(UpsertSql, createdBy);
}
