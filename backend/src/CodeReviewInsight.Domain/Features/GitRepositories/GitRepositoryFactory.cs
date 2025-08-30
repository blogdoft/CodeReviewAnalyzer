using System.Data;

namespace CodeReviewInsight.Domain.Features.GitRepositories;

public class GitRepositoryFactory
{
    private string? _name;
    private Uri? _url;
    private Guid? _id = null;

    public GitRepository Build() => new()
    {
        Id = _id ?? Guid.NewGuid(),
        Name = _name ?? throw new NoNullAllowedException("You must provide a name for this repository"),
        Url = _url ?? throw new NoNullAllowedException("You must provide a URL for this repository"),
    };

    public GitRepositoryFactory WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public GitRepositoryFactory WithName(string name)
    {
        _name = name;
        return this;
    }

    public GitRepositoryFactory WithUrl(string url)
    {
        _url = new Uri(url);
        return this;
    }
}
