namespace CodeReviewAnalyzer.Application.Integrations.Models;

public class User
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

        if (obj is not User)
        {
            return false;
        }

        return ((User)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
