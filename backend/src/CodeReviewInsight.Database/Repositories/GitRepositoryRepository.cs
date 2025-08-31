using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Application.Repositories;
using CodeReviewInsight.Domain.Features.GitRepositories;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace CodeReviewInsight.Database.Repositories;

public class GitRepositoryRepository(
    ILogger<GitRepositoryRepository> logger,
    IDatabaseFacade databaseFacade) : IGitRepositoryRepository
{
    private const string Upsert =
        """
            INSERT INTO public.repositories (
                  tenant_id
                , shared_key
                , "name"
                , url
            ) VALUES (
                  (select tn.id from tenants tn where tn.shared_key = @tenantId)
                , @SharedKey
                , @Name
                , @Url
            )
            ON CONFLICT (shared_key)
            DO UPDATE 
            SET 
                  tenant_id=EXCLUDED.tenant_id
                , shared_key=EXCLUDED.shared_key
                , "name"=EXCLUDED."name"
                , url=EXCLUDED.url;

        """;

    private readonly ILogger<GitRepositoryRepository> _logger = logger;
    private readonly IDatabaseFacade _databaseFacade = databaseFacade;

    public async Task BulkUpsertAsync(IEnumerable<GitRepository> extractedRepositories)
    {
        using var transaction = _databaseFacade.GetDbConnection().BeginTransaction();
        foreach (var repository in extractedRepositories)
        {
            try
            {
                await _databaseFacade.ExecuteAsync(Upsert, new
                {
                    repository.TenantId,
                    SharedKey = repository.Id,
                    repository.Name,
                    Url = repository.Url.ToString(),
                });
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(
                    exception: ex,
                    "Error while persisting repository {RepositoryName}: {Message}",
                    repository.Name,
                    ex.Message);
                transaction.Rollback();
                return;
            }
        }

        transaction.Commit();
    }
}
