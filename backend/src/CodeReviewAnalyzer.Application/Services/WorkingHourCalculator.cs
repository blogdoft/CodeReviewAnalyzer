using CodeReviewAnalyzer.Application.Integrations.Models;

namespace CodeReviewAnalyzer.Application.Services;

public class WorkingHourCalculator(
    PeriodTimeSpan morning,
    PeriodTimeSpan afternoon,
    IEnumerable<DateOnly> holidays)
{
    public TimeSpan Calculate(DateTime createdAt, DateTime closedAt)
    {
        var interval = closedAt - createdAt;
        return interval;
    }
}
