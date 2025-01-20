namespace CodeReviewAnalyzer.Database.Contexts;

public interface IGridReaderFacade : IDisposable
{
    Task<T?> ReadFirstOrDefaultAsync<T>();

    IEnumerable<T> Read<T>(bool buffered = true);

    Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true);
}
