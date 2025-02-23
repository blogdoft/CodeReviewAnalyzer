using CodeReviewAnalyzer.Application.Integrations.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface ICodeRepository
{
    Task AddAsync(CodeRepository codeRepository);
}
