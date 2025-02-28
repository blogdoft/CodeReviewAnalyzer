using AutoFixture;
using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.AzureDevopsItg.Clients;
using CodeReviewAnalyzer.AzureDevopsItg.Clients.Impl;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;


namespace CodeReviewAnalyzer.AzureDevopsItg.Tests.Clients.PullRequestClientTests;

public class BasePullRequestClientTest
{
    public BasePullRequestClientTest()
    {
        ConnectionFactory = Substitute.For<IConnectionFactory>();
        Connection = Substitute.For<IVssConnection>();
        ProjectClient = Substitute.For<ProjectHttpClient>(
            new Uri("https://www.com.br"),
            Substitute.For<VssCredentials>());
        GitClient = Substitute.For<GitHttpClient>(
            new Uri("https://www.com.br"),
            Substitute.For<VssCredentials>());

        var project = new AutoFaker<TeamProject>().Generate();

        ConnectionFactory
            .CreateConnection(Arg.Any<Configuration>())
            .Returns(Connection);

        Connection
            .GetClientAsync<ProjectHttpClient>()
            .Returns(Task.FromResult(ProjectClient));
        Connection
            .GetClientAsync<GitHttpClient>()
            .Returns(Task.FromResult(GitClient));

        ProjectClient
            .GetProject(Arg.Any<string>())
            .Returns(Task.FromResult(project));

        GitClient
            .GetRepositoriesAsync(project.Id, includeHidden: false)
            .Returns(Task.FromResult(new List<GitRepository>()));
    }

    public IConnectionFactory ConnectionFactory { get; }

    public IVssConnection Connection { get; }

    public ProjectHttpClient ProjectClient { get; }

    public GitHttpClient GitClient { get; }

    [Fact]
    public async Task Should_ReturnAllCompletedPullRequestsAsync()
    {
        // Given
        var configuration = new Fixture()
            .Create<Configuration>();
        var client = new PullRequestClient(ConnectionFactory);

        // When
        var pullRequests = client.GetPullRequestsAsync(
            configuration,
            minTime: DateTime.Now.AddDays(-1),
            maxTime: DateTime.Now);

        // Then
        await foreach (var item in pullRequests)
        {
            pullRequests.ShouldBeAssignableTo<IAsyncEnumerable<PullRequest>>();
        }
    }
}
