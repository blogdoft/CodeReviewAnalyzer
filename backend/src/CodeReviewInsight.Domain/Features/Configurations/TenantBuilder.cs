using CodeReviewInsight.Domain.Features.Configurations.Entities;
using System.Data;

namespace CodeReviewInsight.Domain.Features.Configurations;

public class TenantBuilder
{
    private Guid _id = Guid.Empty;
    private string? _name;
    private IEnumerable<DataSource> _dataSources = [];
    private bool _active = true;

    public Tenant Build() => new Tenant(
        _id,
        name: _name ?? throw new NoNullAllowedException("You are trying to create a unnamed Tenant."),
        _dataSources,
        _active);

    public TenantBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public TenantBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public TenantBuilder WithDataSource(IEnumerable<DataSource> dataSources)
    {
        _dataSources = dataSources;
        return this;
    }

    public TenantBuilder WithActive()
    {
        _active = true;

        return this;
    }

    public TenantBuilder WithDeactivate()
    {
        _active = false;
        return this;
    }
}
