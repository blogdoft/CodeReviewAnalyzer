using CodeReviewInsight.Domain.Features.Configurations.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeReviewAnalyzer.Api.Features.ApplicationSetup.Models;

/// <summary>
/// Create/update Azure DevOps configuration
/// </summary>
public class AzureDevOpsDto
{
    /// <summary>
    /// Configuration name.
    /// </summary>
    /// <example>My DevOps.</example>
    [Required(AllowEmptyStrings = false)]
    [MinLength(1)]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// This tenant is active and allowed to query on system.
    /// </summary>
    /// <example>true</example>
    [Required]
    public bool Active { get; set; }

    /// <summary>
    /// The Azure DevOps Organization URL.
    /// </summary>
    /// <example>https://dev.azure.com/MyCompany</example>
    [Required(AllowEmptyStrings = false)]
    [MinLength(1)]
    [MaxLength(255)]
    public string DevOpsUrl { get; set; } = string.Empty;

    /// <summary>
    /// Personal Access Token for Azure DevOps communication.
    /// </summary>
    /// <example>ajswq34234hhufiu32fiu4y2f34yfi2uyf34i2uy3gf4i2uy3f4i2</example>
    [Required(AllowEmptyStrings = false)]
    [MinLength(1)]
    [MaxLength(255)]
    public string Pat { get; set; } = string.Empty;

    /// <summary>
    /// An array contaning all projects that may be loaded.
    /// </summary>
    /// <example>["Project1", "Project2"]</example>
    public IEnumerable<string> Projects { get; set; } = [];

    /// <summary>
    /// An aray containing all areas that may be loaded.
    /// </summary>
    /// <example>["area/squad1","area/squad2","area2/squad1"]</example>
    public IEnumerable<string> Areas { get; set; } = [];

    internal static AzureDevOpsDto From(AzureDevOps dataSource) => new()
    {
        Name = dataSource.Name,
        DevOpsUrl = dataSource.DevOpsUrl.ToString(),
        Pat = dataSource.Pat,
        Projects = dataSource.Projects,
        Areas = dataSource.Areas,
        Active = dataSource.Active,
    };

    internal DataSource To() => new CodeReviewInsight.Domain.Features.Configurations.Entities.AzureDevOps(
        Name,
        new Uri(DevOpsUrl),
        Pat,
        Projects,
        Areas,
        Active);
}
