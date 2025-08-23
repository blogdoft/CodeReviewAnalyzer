namespace CodeReviewInsight.Application.Integrations.Models;

public class IntegrationPerson
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required bool Active { get; init; } = true;

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is not IntegrationPerson)
        {
            return false;
        }

        return ((IntegrationPerson)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
