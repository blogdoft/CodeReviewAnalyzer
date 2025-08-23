namespace CodeReviewInsight.Domain.Features.People;

public class Person
{
    internal Person()
    {
    }

    public Guid Id { get; init; }

    public string? ExternalId { get; init; }

    public required string Name { get; init; }

    public Uri? AvatarUrl { get; init; }
}
