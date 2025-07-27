using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Domain.Tests.Entities.TenantTests;

public class TenantTests
{
    [Fact]
    public void Should_CreateAnTenant_When_AllDataIsAvailable()
    {
        // Given
        var id = Guid.NewGuid();
        var name = BogusFixture.Get().Random.String();
        var active = BogusFixture.Get().Random.Bool();
        var dataSource = new AutoFaker<AzureDevOps>().Generate(2);

        // When
        var tenant = new Tenant(id, name, dataSource, active);

        // Then
        tenant.Id.ShouldBe(id);
        tenant.Name.ShouldBe(name);
        tenant.Active.ShouldBe(active);
        tenant.DataSource.ShouldBe(dataSource);
    }

    [Fact]
    public void Should_BeActive_when_NotDefineAnExplicitValue()
    {
        // Given
        var id = Guid.NewGuid();
        var name = BogusFixture.Get().Random.String();
        var dataSource = new AutoFaker<AzureDevOps>().Generate(2);

        // When
        var tenant = new Tenant(id, name, dataSource);

        // Then
        tenant.Active.ShouldBeTrue();
    }
}
