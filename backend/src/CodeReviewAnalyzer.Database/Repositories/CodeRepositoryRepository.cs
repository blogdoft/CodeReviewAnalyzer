using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Database.Contexts;

namespace CodeReviewAnalyzer.Database.Repositories;

public class CodeRepositoryRepository(IDatabaseFacade databaseFacade) : ICodeRepository
{
    private const string UpInsert =
        """
            INSERT INTO public."REPOSITORIES" (
                  external_id
                , "name"
                , remote_url
                , created_at
            ) values (
                  @ExternalId
                , @Name
                , @RemoteUrl
                , @CreatedAt) 
            ON CONFLICT (external_id)
            DO UPDATE SET 
                  "name" = @Name
                , remote_url = @RemoteUrl
                , created_at = @CreatedAt
                , updated_at = @UpdatedAt;

        """;

    public async Task AddAsync(CodeRepository codeRepository) =>
        await databaseFacade.ExecuteAsync(UpInsert, new
        {
            codeRepository.ExternalId,
            codeRepository.Name,
            RemoteUrl = codeRepository.Url,
            CreatedAt = DateTime.UtcNow,
            updatedAt = DateTime.UtcNow,
        });
}
