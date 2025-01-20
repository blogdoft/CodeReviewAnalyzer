using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface IConfigurations
{
    Task<IEnumerable<Configuration>> GetAllAsync();
}
