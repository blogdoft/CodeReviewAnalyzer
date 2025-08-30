using CodeReviewInsight.Application.TenantFeature;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Services.Processors;

public class TenantProcessor(
    ITenantRepository repository,
    IDataSourceProcessor dataSourceProcessor)
{
    private readonly ITenantRepository _repository = repository;
    private readonly IDataSourceProcessor _dataSourceProcessor = dataSourceProcessor;

    public async Task ProcessAllTenantsAsync()
    {
        var tenants = await _repository.GetAllAsync();
        foreach (var tenant in tenants)
        {
            await ProcessTenantAsync(tenant);
        }
    }

    public async Task ProcessTenantAsync(Tenant tenant)
    {
        await _dataSourceProcessor.ProcessAsync(tenant.DataSource);
    }
}
