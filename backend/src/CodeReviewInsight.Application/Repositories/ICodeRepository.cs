using CodeReviewInsight.Application.Integrations.Models;

namespace CodeReviewInsight.Application.Repositories;

public interface ICodeRepository
{
    Task AddAsync(CodeRepository codeRepository);
}
