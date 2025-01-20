namespace CodeReviewAnalyzer.Application.Models;

public class DayOff
{
    public required Guid ExternalId { get; init; }

    public required string Description { get; init; }

    public required DateOnly Date { get; init; }

    public required short Year { get; init; }

    public required short Month { get; init; }
}
