namespace CodeReviewAnalyzer.Application.Models;

public class Person
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public static Person CreateAsLookup(Guid id) => new()
    {
        Id = id,
        Name = string.Empty,
    };
}
