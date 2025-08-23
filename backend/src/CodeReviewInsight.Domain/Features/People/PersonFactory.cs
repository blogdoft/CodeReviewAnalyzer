namespace CodeReviewInsight.Domain.Features.People;

public class PersonFactory
{
    private Guid _id;
    private string? _externalId;
    private string _name = string.Empty;
    private Uri? _avatarUri;

    public Person Build() => new()
    {
        Id = _id,
        Name = _name,
        ExternalId = _externalId ?? _id.ToString(),
        AvatarUrl = _avatarUri ?? new Uri("www.com.br"),
    };

    public Person BuildAsLookup() => new()
    {
        Id = _id,
        Name = _name,
    };

    public PersonFactory WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PersonFactory WithExternalId(string? externalId)
    {
        _externalId = externalId;
        return this;
    }

    public PersonFactory WithName(string? name)
    {
        _name = name ?? "undefined";
        return this;
    }

    public PersonFactory WithAvatarUri(Uri? avatarUri)
    {
        _avatarUri = avatarUri;
        return this;
    }

    public PersonFactory WithAvatarUri(string? avatarUri)
    {
        _avatarUri = string.IsNullOrEmpty(avatarUri)
            ? new Uri("www.com.br")
            : new Uri(avatarUri);

        return this;
    }
}
