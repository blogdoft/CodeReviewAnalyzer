using CodeReviewAnalyzer.Application.Integrations.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface IUsers
{
    Task Upsert(User createdBy);
}
