namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class IntegrationUser
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

        if (obj is not IntegrationUser)
        {
            return false;
        }

        return ((IntegrationUser)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
