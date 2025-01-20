namespace CodeReviewAnalyzer.Application.Integrations.Models;

public readonly struct PeriodTimeSpan(TimeSpan begin, TimeSpan end)
{
    public TimeSpan Begin { get; } = begin;

    public TimeSpan End { get; } = end;
}
