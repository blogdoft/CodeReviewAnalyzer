using CodeReviewAnalyzer.Application.Integrations.Models;

namespace CodeReviewAnalyzer.Application.Reports;

public interface ICodeRepository
{
    Task AddAsync(CodeRepository codeRepository);
}
