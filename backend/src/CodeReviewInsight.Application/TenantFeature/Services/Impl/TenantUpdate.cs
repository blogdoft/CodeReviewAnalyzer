using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.TenantFeature.Services.Impl;

public class TenantUpdate : ITenantUpdate
{
    private readonly ITenantRepository _repository;

    public TenantUpdate(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<Tenant> ExecuteAsync(Tenant tenant)
    {
        Tenant updatedTenant = await _repository.UpdateAsync(tenant);

        return updatedTenant;
    }
}
