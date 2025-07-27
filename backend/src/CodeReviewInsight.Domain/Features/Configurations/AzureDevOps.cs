using CodeReviewInsight.Domain.Enums;

namespace CodeReviewInsight.Domain.Features.Configurations.Entities;

public class AzureDevOps : DataSource
{
    public AzureDevOps(
        string name,
        Uri devOpsUrl,
        string pat,
        IEnumerable<string> projects,
        IEnumerable<string> areas,
        bool active = true)
        : base(name, active)
    {
        DevOpsUrl = devOpsUrl;
        Pat = pat;
        Projects.AddRange(projects);
        Areas.AddRange(areas);
    }

    public Uri DevOpsUrl { get; }

    public string Pat { get; }

    public List<string> Projects { get; } = [];

    public List<string> Areas { get; } = [];

    public override IntegrationType GetIntegrationType() =>
        IntegrationType.AzureDevops;
}
