namespace CodeReviewAnalyzer.Database.Exceptions;

[Serializable]
public class PaginatedSqlBuilderException : Exception
{
    public PaginatedSqlBuilderException()
    {
    }

    public PaginatedSqlBuilderException(string message)
        : base(message)
    {
    }

    public PaginatedSqlBuilderException(string message, Exception inner)
        : base(message, inner)
    {
    }
}