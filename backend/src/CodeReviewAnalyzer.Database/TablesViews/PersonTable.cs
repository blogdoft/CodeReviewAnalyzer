using CodeReviewInsight.Domain.Features.People;

namespace CodeReviewAnalyzer.Database.TablesViews;

public class PersonTable
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string? ExternalId { get; set; }

    public string? Name { get; set; }

    public string? AvatarUrl { get; set; }

    public Person ToEntity() => new PersonFactory()
        .WithId(Id)
        .WithExternalId(ExternalId)
        .WithName(Name)
        .WithAvatarUri(AvatarUrl)
        .Build();
}
