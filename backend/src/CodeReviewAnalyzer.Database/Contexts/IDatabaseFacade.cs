namespace CodeReviewAnalyzer.Database.Contexts;

public interface IDatabaseFacade : IDisposable
{
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? param = null);

    Task<int> ExecuteAsync(string sql, object? param = null);

    Task<TReturn?> ExecuteScalarAsync<TReturn>(string sql, object? param = null);

    Task<IGridReaderFacade> QueryMultipleAsync(
        string sql,
        object? param);
}
