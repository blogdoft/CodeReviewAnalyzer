using CodeReviewInsight.Application.Integrations.Models;

namespace CodeReviewInsight.Application.Services;

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
