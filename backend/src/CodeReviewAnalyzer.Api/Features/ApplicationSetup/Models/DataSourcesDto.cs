namespace CodeReviewAnalyzer.Api.Features.ApplicationSetup.Models;

/// <summary>
/// Represents all data sources supported by a Tenant.
/// </summary>
public class DataSourcesDto
{
    /// <summary>
    /// All Azure DevOps owned by Tenant.
    /// </summary>
    public IEnumerable<AzureDevOpsDto>? AzureDevOps { get; set; } = [];
}
