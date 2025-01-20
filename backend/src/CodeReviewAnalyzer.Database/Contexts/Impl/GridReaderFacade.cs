using System.Diagnostics.CodeAnalysis;
using static Dapper.SqlMapper;

namespace CodeReviewAnalyzer.Database.Contexts.Impl;

[ExcludeFromCodeCoverage]
internal class GridReaderFacade : IGridReaderFacade
{
    private readonly GridReader _gridReader;
    private bool _disposed;

    internal GridReaderFacade(GridReader gridReader) =>
        _gridReader = gridReader;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<T> Read<T>(bool buffered = true) =>
        _gridReader.Read<T>(buffered);

    public Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true)
        => _gridReader.ReadAsync<T>(buffered);

    public Task<T?> ReadFirstOrDefaultAsync<T>() =>
        _gridReader.ReadFirstOrDefaultAsync<T>();

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _gridReader.Dispose();
        }

        _disposed = true;
    }
}
