using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Database.Tests.Repositories.TenantRepositoryTests;

public class CreateAsyncTests : BaseTenantRepositoryTests
{
    [Fact]
    public async Task Should_CallInsertWithCorrectParameters_AndReturnId_When_CreateTenantAsync()
    {
        // Given
        var expectedId = Guid.NewGuid();
        var tenant = new Tenant(id: expectedId, name: "Acme", dataSource: Array.Empty<DataSource>(), active: true);
        DatabaseFacade
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object?>())
            .Returns(Task.FromResult(1));
        var repository = BuildTenantRepository();

        // When
        var result = await repository.CreateAsync(tenant);

        // Then
        result.ShouldBe(expectedId);
        await DatabaseFacade.Received(1).ExecuteAsync(
            Arg.Is<string>(sql => sql.Contains("INSERT INTO", StringComparison.OrdinalIgnoreCase)),
            Arg.Is<object>(p =>
                GetProp<Guid>(p, "SharedKey") == expectedId &&
                GetProp<string>(p, "Name") == "Acme" &&
                GetProp<bool>(p, "Active"))
        );
    }
}
