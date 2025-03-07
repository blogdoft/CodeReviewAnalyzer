using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Repositories;
using CodeReviewAnalyzer.Database.Contexts;

namespace CodeReviewAnalyzer.Database.Repositories;

public class CodeRepositoryRepository(IDatabaseFacade databaseFacade) : ICodeRepository
{
    private const string UpInsert =
        """
            INSERT INTO public."REPOSITORIES" (
                  external_id
                , "name"
                , name_sh
                , remote_url
                , created_at
            ) values (
                  @ExternalId
                , @Name
                , @NameSh
                , @RemoteUrl
                , @CreatedAt) 
            ON CONFLICT (external_id)
            DO UPDATE SET 
                  "name" = @Name
                , name_sh = @NameSh
                , remote_url = @RemoteUrl
                , created_at = @CreatedAt
                , updated_at = @UpdatedAt;

        """;

    public async Task AddAsync(CodeRepository codeRepository) =>
        await databaseFacade.ExecuteAsync(UpInsert, new
        {
            codeRepository.ExternalId,
            codeRepository.Name,
            NameSh = codeRepository.Name.ToUpper(),
            RemoteUrl = codeRepository.Url,
            CreatedAt = DateTime.UtcNow,
            updatedAt = DateTime.UtcNow,
        });
}
