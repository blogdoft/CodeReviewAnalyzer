using CodeReviewInsight.Domain.Features.Configurations.Entities;
using NSubstitute.ExceptionExtensions;
using System.Data;

namespace CodeReviewInsight.Database.Tests.Repositories.TenantRepositoryTests;

public class UpdateAsyncTest : BaseTenantRepositoryTests
{
    [Fact]
    public async Task Should_OpenTransactionAndCommit_When_UpdateSucceedsAsync()
    {
        // Given
        var tenant = new Tenant(id: Guid.NewGuid(), name: "X", dataSource: Array.Empty<DataSource>(), active: true);
        DatabaseFacade
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<IDbTransaction>())
            .Returns(Task.FromResult(1));
        var repository = BuildTenantRepository();

        // When
        var returned = await repository.UpdateAsync(tenant);

        // Then
        returned.ShouldBe(tenant);
        Connection.Received(1).BeginTransaction();
        Transaction.Received(1).Commit();
        Transaction.DidNotReceive().Rollback();
    }
    [Fact]
    public async Task Should_RollbackAndRethrow_When_UpdateHandlerThrowsAsync()
    {
        // Given
        var tenant = new Tenant(
            id: Guid.NewGuid(),
            name: "X",
            dataSource: [new AzureDevOps(name: "Y", new Uri("https://www.com.br"), pat: Guid.NewGuid().ToString(), [], [], true)], active: true);
        DatabaseFacade.GetDbConnection().Returns(Connection);
        Connection.BeginTransaction().Returns(Transaction);
        DatabaseFacade
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object?>())
            .ReturnsForAnyArgs(Task.FromResult(1));
        DatabaseFacade
            .ExecuteScalarAsync<int>(Arg.Any<string>(), Arg.Any<object?>())
            .ThrowsAsyncForAnyArgs<InvalidOperationException>();
        var sut = BuildTenantRepository();

        // When + Then
        var ex = await Should.ThrowAsync<InvalidOperationException>(() => sut.UpdateAsync(tenant));
        ex.ShouldBeOfType<InvalidOperationException>();
        Transaction.Received(1).Rollback();
        Transaction.DidNotReceive().Commit();
        Transaction.Received(1).Dispose();
    }
}
