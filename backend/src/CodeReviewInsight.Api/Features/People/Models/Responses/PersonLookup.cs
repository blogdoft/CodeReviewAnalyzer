using CodeReviewInsight.Domain.Features.People;

namespace CodeReviewInsight.Api.Features.People.Models.Responses;

public class PersonLookup
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    internal static PersonLookup From(Person person) => new()
    {
        Id = person.Id,
        Name = person.Name,
    };
}
