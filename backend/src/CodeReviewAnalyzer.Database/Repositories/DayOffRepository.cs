using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Repositories;

namespace CodeReviewAnalyzer.Database.Repositories;

public class DayOffRepository(IDatabaseFacade databaseFacade) : IDayOff
{
    private const string QueryDayOff =
        """
            SELECT "ID"
                , "EXTERNAL_ID"
                , "DESCRIPTION"
                , "DATE"
                , "YEAR"
                , "MONTH" 
            FROM 
                "DAY_OFF"
            WHERE
                "DATE" between @From and @To
        """;

    private readonly IDatabaseFacade _databaseFacade = databaseFacade;

    public async Task<IEnumerable<DayOff>> GetAllAsync(
        DateOnly from,
        DateOnly to) =>
        await _databaseFacade.QueryAsync<DayOff>(
            QueryDayOff,
            new
            {
                From = from,
                To = to,
            });
}
