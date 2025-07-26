using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewAnalyzer.Application.TenantFeature.Services.Impl;

public class TenantAdd : ITenantAdd
{
    private readonly ITenantRepository _tenantRepository;

    public TenantAdd(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Guid> Execute(Tenant tenant)
    {
        return await _tenantRepository.CreateAsync(tenant);
    }
}
