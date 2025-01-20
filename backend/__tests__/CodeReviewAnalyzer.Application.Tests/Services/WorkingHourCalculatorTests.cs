using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Services;

namespace CodeReviewAnalyzer.Application.Tests.Integrations.Models.PullRequestsTests;

public class WorkingHourCalculatorTests
{
    private readonly PeriodTimeSpan _morningWorkingTime = new(TimeSpan.FromHours(8), TimeSpan.FromHours(12));
    private readonly PeriodTimeSpan _afternoonWorkingTime = new(TimeSpan.FromHours(13), TimeSpan.FromHours(17));

    [Fact]
    public void Should_CalculateOnlyWorkHours_When_OpenClosedAtSameDay()
    {
        // Given
        var expedientBegin = new DateTime(year: 2025, month: 1, day: 31, hour: 8, minute: 0, second: 0, DateTimeKind.Unspecified);
        var expedientEnd = new DateTime(year: 2025, month: 1, day: 31, hour: 17, minute: 0, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(expedientBegin, expedientEnd);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(8));
    }

    [Fact]
    public void Should_CalculateOnlyWorkingHours_When_ClosedInOtherDayIntoExpedient()
    {
        // Given
        var yesterday = new DateTime(year: 2025, month: 1, day: 30, hour: 8, minute: 0, second: 0, DateTimeKind.Unspecified);
        var todayBeginningDay = new DateTime(year: 2025, month: 1, day: 31, hour: 8, minute: 30, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(yesterday, todayBeginningDay);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)));
    }

    [Fact]
    public void Should_CalculateOnlyWorkingHours_When_HasWeekendInTheMiddle()
    {
        // Given
        var friday = new DateTime(year: 2025, month: 1, day: 31, hour: 8, minute: 0, second: 0, DateTimeKind.Unspecified);
        var nextMonday = new DateTime(year: 2025, month: 2, day: 3, hour: 8, minute: 30, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(friday, nextMonday);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)));
    }

    [Fact]
    public void Should_CalculateOnlyWorkingHours_When_HasAnHoliday()
    {
        // Given
        var friday = new DateTime(year: 2025, month: 1, day: 31, hour: 8, minute: 0, second: 0, DateTimeKind.Unspecified);
        var nextTuesday = new DateTime(year: 2025, month: 2, day: 5, hour: 8, minute: 30, second: 0, DateTimeKind.Unspecified);
        var holidays = new List<DateOnly>
        {
            new(year: 2025, month: 2, day: 3),
            new(year: 2025, month: 2, day: 4),
        };
        var calculator = new WorkingHourCalculator(
            _morningWorkingTime,
            _afternoonWorkingTime,
            holidays);

        // When
        var waitingTime = calculator.Calculate(friday, nextTuesday);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)));
    }

    [Fact]
    public void Should_CalculateOverTime_When_ClosedDuringLunchTime()
    {
        // Given
        var friday = new DateTime(year: 2025, month: 1, day: 31, hour: 11, minute: 59, second: 0, DateTimeKind.Unspecified);
        var nextTuesday = new DateTime(year: 2025, month: 1, day: 31, hour: 12, minute: 5, second: 0, DateTimeKind.Unspecified);
        var holidays = Array.Empty<DateOnly>();
        var calculator = new WorkingHourCalculator(
            _morningWorkingTime,
            _afternoonWorkingTime,
            holidays);

        // When
        var waitingTime = calculator.Calculate(friday, nextTuesday);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromMinutes(6));
    }

    [Fact]
    public void Should_CalculateOverTime_When_ClosedAfterExpedient()
    {
        // Given
        var expedientBegin = new DateTime(year: 2025, month: 1, day: 31, hour: 8, minute: 0, second: 0, DateTimeKind.Unspecified);
        var expedientEndOverTime = new DateTime(year: 2025, month: 1, day: 31, hour: 18, minute: 0, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(
            createdAt: expedientBegin,
            closedAt: expedientEndOverTime);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(9));
    }

    [Fact]
    public void Should_CalculateOverTime_When_ClosedAfterExpedientAtOtherDay()
    {
        // Given
        var expedientBegin = new DateTime(year: 2025, month: 1, day: 31, hour: 8, minute: 0, second: 0, DateTimeKind.Unspecified);
        var expedientEndOverTime = new DateTime(year: 2025, month: 2, day: 3, hour: 17, minute: 01, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(
            createdAt: expedientBegin,
            closedAt: expedientEndOverTime);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(1)));
    }

    [Fact]
    public void Should_CalculateOverTime_When_OpenedBeforeExpedient()
    {
        // Given
        var openedBeforeExpedient = new DateTime(year: 2025, month: 1, day: 31, hour: 7, minute: 0, second: 0, DateTimeKind.Unspecified);
        var expedientEnd = new DateTime(year: 2025, month: 1, day: 31, hour: 17, minute: 0, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(openedBeforeExpedient, expedientEnd);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(9));
    }

    [Fact]
    public void Should_AddThirteenMinutesOverTime_When_CloseBeforeExpedient()
    {
        // Given
        var yesterday = new DateTime(year: 2025, month: 1, day: 30, hour: 8, minute: 0, second: 0, DateTimeKind.Unspecified);
        var closedBeforeExpedient = new DateTime(year: 2025, month: 1, day: 31, hour: 7, minute: 30, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();
        // When
        var waitingTime = calculator.Calculate(yesterday, closedBeforeExpedient);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)));
    }

    [Fact]
    public void Should_NotAddOverTime_When_OpenedAfterExpedient()
    {
        // Given
        var yesterday = new DateTime(year: 2025, month: 1, day: 30, hour: 19, minute: 0, second: 0, DateTimeKind.Unspecified);
        var closedBeforeExpedient = new DateTime(year: 2025, month: 1, day: 31, hour: 8, minute: 30, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(yesterday, closedBeforeExpedient);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromMinutes(30));
    }

    [Fact]
    public void Should_HaveThirtyMinutes_When_OpenedAfterClosedBeforeExpedient()
    {
        // Given
        var openedAfterExpedient = new DateTime(year: 2025, month: 1, day: 30, hour: 19, minute: 0, second: 0, DateTimeKind.Unspecified);
        var closedBeforeExpedient = new DateTime(year: 2025, month: 1, day: 31, hour: 6, minute: 30, second: 0, DateTimeKind.Unspecified);
        var calculator = BuildWorkingHourCalculator();

        // When
        var waitingTime = calculator.Calculate(openedAfterExpedient, closedBeforeExpedient);

        // Then
        waitingTime.ShouldBe(TimeSpan.FromMinutes(30));
    }

    private WorkingHourCalculator BuildWorkingHourCalculator() =>
        new(
            _morningWorkingTime,
            _afternoonWorkingTime,
            []);
}
