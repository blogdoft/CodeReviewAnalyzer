using CodeReviewInsight.Domain.Features.Configurations.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeReviewAnalyzer.Api.Features.ApplicationSetup.Models.Requests;

public class CreateTenantRequest
{
    /// <summary>
    /// A descriptive anme to identify this tenant.
    /// </summary>
    /// <example>My Company Inc.</example>
    [Required]
    [MaxLength(255)]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    public Tenant To() => new(
        id: Guid.NewGuid(),
        name: Name,
        dataSource: []);
}
