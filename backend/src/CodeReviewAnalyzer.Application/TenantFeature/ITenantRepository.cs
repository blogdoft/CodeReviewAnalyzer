using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewAnalyzer.Application.TenantFeature;

public interface ITenantRepository
{
    Task<Guid> CreateAsync(Tenant tenant);

    Task<Tenant> UpdateAsync(Tenant tenant);

    Task<Tenant> GetByIdAsync(Guid tenantId);
}
