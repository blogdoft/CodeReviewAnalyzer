using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.TenantFeature.Services;

public interface ITenantAdd
{
    Task<Guid> Execute(Tenant tenant);
}
