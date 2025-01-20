namespace CodeReviewAnalyzer.Application.Tests.Services.RetrievePullRequestMetadataTests;

public class BasePullRequestMetadataProcessorTests
{
    // private readonly IPullRequestsClient _pullRequestClient;
    // private readonly IConfigurations _configurations;
    // private readonly IUsers _users;
    // private readonly IPullRequests _pullRequests;

    // private readonly Faker _faker = BogusFixture.Get();

    // public BasePullRequestMetadataProcessorTests()
    // {
    //     _pullRequestClient = Substitute.For<IPullRequestsClient>();
    //     _configurations = Substitute.For<IConfigurations>();
    //     _users = Substitute.For<IUsers>();
    //     _pullRequests = Substitute.For<IPullRequests>();
    // }

    // [Fact]
    // public async Task Should_ProcessPullRequests_When_ThereAreConfigurationsAsync()
    // {
    //     // Given
    //     var configurationsCount = _faker.Random.Int(min: 1, max: 3);
    //     _configurations
    //         .GetAllAsync()
    //         .Returns(BuildConfigurations(configurationsCount));
    //     var metadataProcessor = BuildMetadataProcessor();

    //     // When
    //     await metadataProcessor.ExecuteAsync();

    //     // Then
    //     _pullRequestClient.Received(configurationsCount).GetPullRequestsAsync(
    //         Arg.Any<Configuration>(),
    //         null,
    //         null);
    // }

    // [Fact]
    // public async Task Should_ProcessAllPullRequests_When_ThereIsClosedPullRequestsAsync()
    // {
    //     // Given
    //     var configurationsCount = _faker.Random.Int(min: 1, max: 3);
    //     _configurations
    //         .GetAllAsync()
    //         .Returns(BuildConfigurations(configurationsCount));
    //     var metadataProcessor = BuildMetadataProcessor();

    //     // When
    //     await metadataProcessor.ExecuteAsync();

    //     // Then
    //     _pullRequestClient.Received(configurationsCount).GetPullRequestsAsync(
    //         Arg.Any<Configuration>(),
    //         null,
    //         null);
    // }

    // private PullRequestMetadataProcessor BuildMetadataProcessor() => new(
    //     _pullRequestClient,
    //     _configurations,
    //     _users,
    //     _pullRequests);

    // private IList<Configuration> BuildConfigurations(int count = 1) => Enumerable
    //     .Range(1, count)
    //     .Select(index => new Configuration
    //     {
    //         ProjectName = _faker.Lorem.Word(),
    //         AccessToken = _faker.Internet.Mac(),
    //         Organization = _faker.Company.CompanyName(),
    //     })
    //     .ToList();
}

