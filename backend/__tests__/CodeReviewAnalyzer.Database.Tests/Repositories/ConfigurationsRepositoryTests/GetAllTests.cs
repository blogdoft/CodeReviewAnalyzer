using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Database.Contexts;
using CodeReviewAnalyzer.Database.Repositories;
using System.Data;

namespace CodeReviewAnalyzer.Database.Tests.Repositories.ConfigurationsRepositoryTests;

public class GetAllTests
{
    public GetAllTests()
    {
        ConnectionFactory = Substitute.For<IConnectionFactory>();
        DatabaseFacade = Substitute.For<IDatabaseFacade>();
    }

    public IConnectionFactory ConnectionFactory { get; }

    public IDatabaseFacade DatabaseFacade { get; private set; }

    [Fact]
    public async Task Should_ReturnAllConfigurations_When_ThereIsConfigurationsOnDatabaseAsync()
    {
        // Given
        List<Configuration> storedConfigurations = new()
        {
            { new Configuration()
                {
                    ProjectName = string.Empty,
                    AccessToken= string.Empty,
                    Organization = string.Empty,
                }
            },
        };
        DatabaseFacade
            .QueryAsync<Configuration>(Arg.Any<string>(), Arg.Any<object?>())
            .Returns(Task.FromResult(storedConfigurations.AsEnumerable()));
        var repository = new ConfigurationsRepository(DatabaseFacade);

        // When
        var configurations = await repository.GetAllAsync();

        // Then
        configurations.ShouldBeAssignableTo<IEnumerable<Configuration>>();
        configurations.ShouldBeEquivalentTo(storedConfigurations);
    }

    [Fact]
    public async Task Should_ReturnEmptyConfiguration_When_ThereIsNoConfigurationStoredAsync()
    {
        // Given
        var storedConfigurations = Enumerable.Empty<Configuration>();
        DatabaseFacade
            .QueryAsync<Configuration>(Arg.Any<string>(), Arg.Any<object?>())
            .Returns(Task.FromResult(storedConfigurations.AsEnumerable()));
        var repository = new ConfigurationsRepository(DatabaseFacade);

        // When
        var configurations = await repository.GetAllAsync();

        // Then
        configurations.ShouldBeEmpty();
    }
}
