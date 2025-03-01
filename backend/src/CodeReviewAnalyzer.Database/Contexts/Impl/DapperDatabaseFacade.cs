using Dapper;
using System.Data;

namespace CodeReviewAnalyzer.Database.Contexts.Impl;

public sealed class DapperDatabaseFacade(
    IConnectionFactory connectionFactory) : IDatabaseFacade
{
    private readonly IDbConnection _connection =
        connectionFactory.GetNewConnection();

    public void Dispose() =>
        _connection.Dispose();

    public Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? param = null) =>
        _connection.QueryAsync<T>(sql, param);

    public async Task<T> QueryFirstAsync<T>(
        string sql,
        object? param = null) =>
        await _connection.QueryFirstAsync<T>(sql, param);

    public async Task<int> ExecuteAsync(string sql, object? param = null) =>
        await _connection.ExecuteAsync(sql, param);

    public async Task<TReturn?> ExecuteScalarAsync<TReturn>(string sql, object? param = null) =>
        await _connection.ExecuteScalarAsync<TReturn>(sql, param);

    public async Task<IGridReaderFacade> QueryMultipleAsync(string sql, object? param)
    {
        var multiple = await _connection.QueryMultipleAsync(sql, param);
        return new GridReaderFacade(multiple);
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null) =>
        await _connection.QuerySingleOrDefaultAsync<T>(sql, param);
}
