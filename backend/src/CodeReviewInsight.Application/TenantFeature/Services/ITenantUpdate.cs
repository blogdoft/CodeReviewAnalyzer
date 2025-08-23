using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.TenantFeature.Services;

public interface ITenantUpdate
{
    Task<Tenant> ExecuteAsync(Tenant tenant);
}
