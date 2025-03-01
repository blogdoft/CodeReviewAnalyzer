using System.Text;

namespace CodeReviewAnalyzer.Database.Services;

internal static class PageCountStmt
{
    private const string SqlCount = "select count(1) from ({0}) as counter_result";

    public static StringBuilder BuildCountSql(StringBuilder sql) =>
        new StringBuilder()
            .AppendFormat(SqlCount, sql);
}
