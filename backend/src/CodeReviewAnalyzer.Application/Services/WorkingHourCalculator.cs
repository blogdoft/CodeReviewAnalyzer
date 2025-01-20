using CodeReviewAnalyzer.Application.Integrations.Models;

namespace CodeReviewAnalyzer.Application.Services;

public class WorkingHourCalculator(
    PeriodTimeSpan morning,
    PeriodTimeSpan afternoon,
    IEnumerable<DateOnly> holidays)
{
    private readonly PeriodTimeSpan _morning = morning;
    private readonly PeriodTimeSpan _afternoon = afternoon;
    private readonly IEnumerable<DateOnly> _holidays = holidays;

    public TimeSpan Calculate(DateTime createdAt, DateTime closedAt)
    {
        var totalTime = TimeSpan.Zero;

        totalTime = totalTime.Add(CalculateOverTime(createdAt, closedAt));

        // Loop diário para calcular o tempo em cada dia
        var today = createdAt.Date;

        while (today <= closedAt.Date)
        {
            // Ignorar sábados, domingos e feriados
            var isWorkingDay = IsWorkingDay(today);

            var closedAtWeekend = !isWorkingDay && today == closedAt.Date;

            if (isWorkingDay || closedAtWeekend)
            {
                totalTime = CalculateWorkingTimeDay(createdAt, closedAt, totalTime, today);
            }

            // Avançar para o próximo dia
            today = today.AddDays(1);
        }

        return totalTime;
    }

    private static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;

    private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;

    private TimeSpan CalculateOverTime(DateTime createdAt, DateTime closedAt)
    {
        var totalTime = TimeSpan.Zero;

        // Created before morning begins
        if (createdAt.TimeOfDay < _morning.Begin)
        {
            totalTime += _morning.Begin - createdAt.TimeOfDay;
        }

        // Closed before morning begins
        if (closedAt.TimeOfDay < _morning.Begin)
        {
            totalTime += TimeSpan.FromMinutes(30);
        }

        // Closed during the lunch time
        if (closedAt.TimeOfDay > _morning.End && closedAt.TimeOfDay < _afternoon.Begin)
        {
            totalTime += closedAt.TimeOfDay - _morning.End;
        }

        // Closed after afternoon ends
        if (closedAt.TimeOfDay > _afternoon.End)
        {
            totalTime += closedAt.TimeOfDay - _afternoon.End;
        }

        return totalTime;
    }

    private bool IsWorkingDay(DateTime today) =>
        !(today.DayOfWeek == DayOfWeek.Saturday ||
        today.DayOfWeek == DayOfWeek.Sunday ||
        _holidays.Any(holiday => holiday == DateOnly.FromDateTime(today)));

    private TimeSpan CalculateWorkingTimeDay(DateTime createdAt, DateTime closedAt, TimeSpan totalTime, DateTime today)
    {
        // Define the day begin and end
        var dayBegin = today.Add(_morning.Begin);
        var dayEndAt = today.Add(_afternoon.End);

        if (today == createdAt.Date)
        {
            dayBegin = createdAt;
        }

        if (today == closedAt.Date)
        {
            dayEndAt = closedAt;
        }

        // evaluate morning time
        var actualMorningBegin = Max(dayBegin, today.Add(_morning.Begin));
        var actualMorningEnd = Min(dayEndAt, today.Add(_morning.End));
        if (actualMorningBegin < actualMorningEnd)
        {
            totalTime += actualMorningEnd - actualMorningBegin;
        }

        // evaluate afternoon time
        var actualAfternoonStart = Max(dayBegin, today.Add(_afternoon.Begin));
        var actualAfternoonEnd = Min(dayEndAt, today.Add(_afternoon.End));
        if (actualAfternoonStart < actualAfternoonEnd)
        {
            totalTime += actualAfternoonEnd - actualAfternoonStart;
        }

        return totalTime;
    }
}
