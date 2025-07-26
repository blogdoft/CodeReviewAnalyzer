using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewAnalyzer.Application.TenantFeature.Services;

public interface ITenantAdd
{
    Task<Guid> Execute(Tenant tenant);
}
