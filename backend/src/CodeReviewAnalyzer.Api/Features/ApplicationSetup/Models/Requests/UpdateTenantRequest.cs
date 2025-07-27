using CodeReviewInsight.Domain.Features.Configurations.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeReviewAnalyzer.Api.Features.ApplicationSetup.Models.Requests;

public class UpdateTenantRequest
{
    /// <summary>
    /// A descriptive name to identify this tenant.
    /// </summary>
    /// <example>My Company Inc.</example>
    [Required]
    [MaxLength(255)]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Inform if the related Tenant is active.
    /// </summary>
    /// <example>true</example>
    [Required]
    public bool Active { get; set; } = false;

    /// <summary>
    /// A list of all data sources owned by Tenant.
    /// </summary>
    public DataSourcesDto? DataSources { get; set; }

    public Tenant To(Guid tenantId)
    {
        var azureDevOps = DataSources?.AzureDevOps?.Select(ds => ds.To()) ?? [];

        return new Tenant(id: tenantId, name: Name, azureDevOps);
    }
}
