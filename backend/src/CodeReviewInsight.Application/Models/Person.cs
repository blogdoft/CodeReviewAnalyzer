namespace CodeReviewInsight.Application.Models;

public class Person
{
    public Guid Id { get; init; }

    public string? ExternalId { get; init; }

    public required string Name { get; init; }

    public Uri? AvatarUrl { get; init; }

    public static Person CreateAsLookup(Guid id) => new()
    {
        Id = id,
        Name = string.Empty,
    };
}
