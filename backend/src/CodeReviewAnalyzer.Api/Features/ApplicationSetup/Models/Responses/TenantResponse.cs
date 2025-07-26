using CodeReviewAnalyzer.Api.Features.ApplicationSetup.Models;
using CodeReviewInsight.Domain.Enums;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewAnalyzer.Api.Features.ApplicationSetup.Responses;

public class TenantResponse
{
    /// <summary>
    /// Tenant unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// A descriptive name to identify this tenant.
    /// </summary>
    /// <example>My Company Inc.</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A list of all data sources owned by Tenant.
    /// </summary>
    public DataSourcesDto? DataSources { get; set; }

    public static TenantResponse From(Tenant tenant) => new()
    {
        Id = tenant.Id,
        Name = tenant.Name,
        DataSources = new()
        {
            AzureDevOps = tenant.DataSource
                .Where(ds => ds.GetIntegrationType() == IntegrationType.AzureDevops)
                .Select(ds => AzureDevOpsDto.From((AzureDevOps)ds)),
        },
    };
}
