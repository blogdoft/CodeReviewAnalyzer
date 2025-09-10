using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using System.Data;

namespace CodeReviewInsight.Domain.Features.GitRepositories;

public class GitRepositoryFactory
{
    private string? _name;
    private Uri? _url;
    private Guid? _id = null;
    private TenantId? _tenantId;
    private string? _externalId;

    public GitRepository Build()
    {
        var id = _id ?? Guid.NewGuid();
        return new()
        {
            Id = id,
            ExternalId = _externalId ?? id.ToString(),
            Name = _name ?? throw new NoNullAllowedException("You must provide a name when creating a repository."),
            Url = _url ?? throw new NoNullAllowedException("You must provide a URL when creating a repository."),
            TenantId = _tenantId ?? throw new NoNullAllowedException("You must provide a TenantId when creating a repository."),
        };
    }

    public GitRepositoryFactory WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public GitRepositoryFactory WithExternalId(string externalId)
    {
        _externalId = externalId;
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

    public GitRepositoryFactory WithTenant(Tenant tenant)
    {
        _tenantId = tenant.Id;
        return this;
    }

    public GitRepositoryFactory WithTenant(TenantId tenantId)
    {
        _tenantId = tenantId;
        return this;
    }
}
