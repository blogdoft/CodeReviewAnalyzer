using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewAnalyzer.Application.TenantFeature.Services;

public interface ITenantUpdate
{
    Task<Tenant> ExecuteAsync(Tenant tenant);
}
