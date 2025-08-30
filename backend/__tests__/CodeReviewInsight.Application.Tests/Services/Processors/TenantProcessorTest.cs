using CodeReviewInsight.Application.Services.Processors;
using CodeReviewInsight.Application.TenantFeature;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Tests.Services.Processors;

public class TenantProcessorTest
{
    public TenantProcessorTest()
    {
        TenantRepository = Substitute.For<ITenantRepository>();
        DataSourceProcessor = Substitute.For<IDataSourceProcessor>();
    }

    internal ITenantRepository TenantRepository { get; }
    internal IDataSourceProcessor DataSourceProcessor { get; }

    [Fact]
    public async Task Should_ProcessAllDataSources_When_ThereAreTenantsRegisteredAsync()
    {
        // Given
        var tenants = TenantFixture.Build(2);
        var totalDataSources = tenants.Count;
        TenantRepository
            .GetAllAsync()
            .Returns(tenants);
        DataSourceProcessor
            .ProcessAsync(Arg.Any<Tenant>())
            .Returns(Task.CompletedTask);

        var processor = BuildTenantProcessor();

        // When
        await processor.ProcessAllTenantsAsync();

        // Then
        await TenantRepository.Received(1).GetAllAsync();
        await DataSourceProcessor.Received(totalDataSources).ProcessAsync(Arg.Any<Tenant>());
    }

    [Fact]
    public async Task Should_ProcessASingleTenantAsync()
    {
        // Given
        var tenant = TenantFixture.Build();
        DataSourceProcessor
            .ProcessAsync(Arg.Any<Tenant>())
            .Returns(Task.CompletedTask);

        var processor = BuildTenantProcessor();

        // When
        await processor.ProcessTenantAsync(tenant);

        // Then
        await TenantRepository.DidNotReceive().GetAllAsync();
        await DataSourceProcessor.Received(1).ProcessAsync(Arg.Any<Tenant>());
    }

    private TenantProcessor BuildTenantProcessor() => new(
        TenantRepository,
        DataSourceProcessor);
}
