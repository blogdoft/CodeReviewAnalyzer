using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;

namespace CodeReviewAnalyzer.Database.Repositories;

internal sealed class ConfigurationsRepository(IDatabaseFacade databaseFacade) : IConfigurations
{
    private readonly IDatabaseFacade _databaseFacade = databaseFacade;

    public async Task<IEnumerable<Configuration>> GetAllAsync() =>
        await _databaseFacade.QueryAsync<Configuration>(
            sql: """SELECT "ID", "CONFIGURATION_ID", "ORGANIZATION" as Organization, "PROJECT_NAME" as ProjectName, "PERSONAL_ACCESS_TOKEN" as AccessToken FROM public."CONFIGURATION";""");
}
