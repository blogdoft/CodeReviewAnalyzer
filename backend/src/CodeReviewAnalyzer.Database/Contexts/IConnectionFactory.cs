using System.Data;

namespace CodeReviewAnalyzer.Database.Contexts;

public interface IConnectionFactory
{
    IDbConnection GetNewConnection();
}
