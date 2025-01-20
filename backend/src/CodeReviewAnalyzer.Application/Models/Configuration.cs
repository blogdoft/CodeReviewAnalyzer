namespace CodeReviewAnalyzer.Application.Models;

public class Configuration
{
    /// <summary>
    /// Project name at Azure Devops.
    /// </summary>
    public required string ProjectName { get; init; }

    public required string Organization { get; init; }

    public required string AccessToken { get; init; }
}
