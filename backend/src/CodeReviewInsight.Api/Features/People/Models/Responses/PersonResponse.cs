using CodeReviewInsight.Domain.Features.People;

namespace CodeReviewInsight.Api.Features.People.Models.Responses;

public class PersonResponse
{
    /// <summary>
    /// A person public and unique identifier.
    /// </summary>
    /// <example>da17b5b0-39ab-4b15-9589-fc25e9747ad0</example>
    public Guid Id { get; set; }

    /// <summary>
    /// An external identifier as was into original data source.
    /// </summary>
    /// <example>24ddc927-1b9c-4556-8183-3c881c4784a6</example>
    public string? ExternalId { get; set; }

    /// <summary>
    /// Person full name.
    /// </summary>
    /// <example>John Doe</example>
    public required string Name { get; set; }

    /// <summary>
    /// Address with a picture as avatar
    /// </summary>
    /// <example>http://www.com.br</example>
    public Uri? AvatarUrl { get; set; }

    public static PersonResponse From(Person person) => new()
    {
        Id = person.Id,
        ExternalId = person.ExternalId,
        Name = person.Name,
        AvatarUrl = person.AvatarUrl,
    };
}
