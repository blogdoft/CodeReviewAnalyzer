namespace CodeReviewAnalyzer.AzureDevopsItg;

public record ClientConfiguration
{
    public required string Organization { get; init; }

    public required string Project { get; init; }

    public required string AccessToken { get; init; }
}
