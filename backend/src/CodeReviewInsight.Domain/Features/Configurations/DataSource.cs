using CodeReviewInsight.Domain.Enums;

namespace CodeReviewInsight.Domain.Features.Configurations.Entities;

public abstract class DataSource
{
    protected DataSource(string name, bool active = true)
    {
        Name = name;
        Active = active;
    }

    public string Name { get; }

    public bool Active { get; }

    public abstract IntegrationType GetIntegrationType();
}
