using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Repositories;

public interface IConfigurations
{
    Task<IEnumerable<Configuration>> GetAllAsync();
}
