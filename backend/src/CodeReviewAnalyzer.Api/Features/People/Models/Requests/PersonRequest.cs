using CodeReviewInsight.Domain.Features.People;
using System.ComponentModel.DataAnnotations;

namespace CodeReviewAnalyzer.Api.Features.People.Models.Requests;

public class PersonRequest
{
    /// <summary>
    /// An external identifier as was into original data source.
    /// </summary>
    /// <example>24ddc927-1b9c-4556-8183-3c881c4784a6</example>
    public string? ExternalId { get; set; }

    /// <summary>
    /// Person full name.
    /// </summary>
    /// <example>John Doe</example>
    [Required(AllowEmptyStrings = false)]
    public required string Name { get; set; }

    /// <summary>
    /// Address with a picture as avatar
    /// </summary>
    /// <example>http://www.com.br</example>
    public string? AvatarUri { get; set; }

    internal Person ToEntity(Guid id) => new PersonFactory()
        .WithId(id)
        .WithExternalId(ExternalId)
        .WithName(Name)
        .WithAvatarUri(AvatarUri)
        .Build();
}
