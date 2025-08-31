using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Database.Repositories;
using CodeReviewInsight.Database.Tests.Fixtures;
using Microsoft.Extensions.Logging;
using Npgsql;
using NSubstitute.ExceptionExtensions;
using System.Data;

namespace CodeReviewInsight.Database.Tests.Repositories.GitRepositoryRepositoryTests;

public class GitRepositoryRepositoryBaseTest
{
    public GitRepositoryRepositoryBaseTest()
    {
        DatabaseFacade = Substitute.For<IDatabaseFacade>();
        DbConnection = Substitute.For<IDbConnection>();
        Transaction = Substitute.For<IDbTransaction>();
        Logger = Substitute.For<ILogger<GitRepositoryRepository>>();

        DatabaseFacade.GetDbConnection().Returns(DbConnection);
        DbConnection.BeginTransaction().Returns(Transaction);
    }

    internal IDatabaseFacade DatabaseFacade { get; }
    internal IDbConnection DbConnection { get; }
    internal IDbTransaction Transaction { get; }
    internal ILogger<GitRepositoryRepository> Logger { get; }

    [Fact]
    public async Task Should_PersistLotsOfRepositories_When_AllRequiredDataIsAvailableAsync()
    {
        // Given
        var gitRepositories = GitRepositoryFake.BuildGitRepository(2);
        var repository = Build();

        // When
        await repository.BulkUpsertAsync(gitRepositories);

        // Then
        await DatabaseFacade.Received(2).ExecuteAsync(Arg.Any<string>(), Arg.Any<object>());
        Transaction.Received(1).Commit();
    }

    [Fact]
    public async Task Should_RollbackAndLog_When_DatabaseErrorOccursAsync()
    {
        // Given
        DatabaseFacade
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object>())
            .ThrowsAsync<NpgsqlException>();
        var gitRepositories = GitRepositoryFake.BuildGitRepository(2);
        var repository = Build();

        // When
        await repository.BulkUpsertAsync(gitRepositories);

        // Then
        Transaction.Received(1).Rollback();
    }

    [Fact]
    public async Task Should_NotRaiseException_When_NpgsqlExceptionOccursAsync()
    {
        // Given
        DatabaseFacade
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object>())
            .ThrowsAsync<NpgsqlException>();
        var gitRepositories = GitRepositoryFake.BuildGitRepository(2);
        var repository = Build();

        // When
        Func<Task> act = () => repository.BulkUpsertAsync(gitRepositories);

        // Then
        await act.ShouldNotThrowAsync();
    }

    private GitRepositoryRepository Build() => new(Logger, DatabaseFacade);
}
