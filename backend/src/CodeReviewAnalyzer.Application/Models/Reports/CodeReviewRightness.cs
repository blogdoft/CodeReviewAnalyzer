namespace CodeReviewAnalyzer.Application.Models.Reports;

public class CodeReviewRightness
{
    public DateTime Start { get; init; }

    public DateTime End { get; init; }

    public decimal Index { get; init; }

    public IEnumerable<CodeReviewRightnessMonthly> Trimester { get; init; } = null!;
}
