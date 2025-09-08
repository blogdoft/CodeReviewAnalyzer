using CodeReviewInsight.AzureDevopsItg.Clients;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using NSubstitute.ExceptionExtensions;
using System.Data;

namespace CodeReviewInsight.AzureDevopsItg.Tests.AzureFacadeTests;

public class BaseAzureFacadeTests
{
    private readonly Faker _faker = BogusFixture.Get();
    private readonly string _project;
    private readonly AzureDevOps _validAzureDevOps;

    public BaseAzureFacadeTests()
    {
        _project = "code-review-insight";
        _validAzureDevOps = new AzureDevOps(
            name: "Test",
            devOpsUrl: new Uri("https://dev.azure.com/blog-do-ft"),
            pat: _faker.Internet.Password(),
            projects: [_project],
            areas: []);
        Logger = Substitute.For<ILogger<AzureFacade>>();
        ConnectionFactory = Substitute.For<IConnectionFactory>();
        Connection = Substitute.For<IVssConnection>();
        ProjectHttpClient = Substitute.For<ProjectHttpClient>(
            _validAzureDevOps.DevOpsUrl,
            new VssCredentials());
        GitHttpClient = Substitute.For<GitHttpClient>(
            _validAzureDevOps.DevOpsUrl,
            new VssCredentials());

        ConnectionFactory
            .CreateConnection(Arg.Any<Uri>(), Arg.Any<string>())
            .Returns(Connection);
        Connection
            .GetClientAsync<ProjectHttpClient>()
            .Returns(ProjectHttpClient);
        Connection
            .GetClientAsync<GitHttpClient>()
            .Returns(GitHttpClient);
        GitHttpClient
            .GetRepositoriesAsync(Arg.Any<Guid>(), includeHidden: false)
            .Returns(new AutoFaker<GitRepository>()
                .RuleFor(gr => gr.RemoteUrl, (fk, _) => fk.Internet.Url())
                .Generate(2));
        ProjectHttpClient
            .GetProject(_project)
            .Returns(new AutoFaker<TeamProject>());
    }

    protected IConnectionFactory ConnectionFactory { get; }
    protected IVssConnection Connection { get; }
    protected ProjectHttpClient ProjectHttpClient { get; }
    protected GitHttpClient GitHttpClient { get; }
    protected ILogger<AzureFacade> Logger { get; }

    [Fact]
    public async Task Should_ReturnRepositories_When_FacadeIsConfiguredAsync()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var facade = Build();

        // When
        var repositories = await facade
            .SetContext(tenantId, _validAzureDevOps)
            .FromProject(_project)
            .GetRepositoriesAsync();

        // Then
        repositories.ShouldNotBeNull();
        repositories.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_ThrowNoNullAllowed_When_DoNotSetTenantIdAsync()
    {
        // Given
        var facade = Build();

        // When
        Func<Task> act = async () =>
        {
            var repo = await facade
                .FromProject(_project)
                .GetRepositoriesAsync();
            _ = repo.ToList();
        };

        // Then
        await act.ShouldThrowAsync<NoNullAllowedException>();
    }

    [Fact]
    public async Task Should_LogError_When_ProjectDoesNotExistAsync()
    {
        // Given
        ProjectHttpClient
            .GetProject(_project)
            .ThrowsAsync<ProjectDoesNotExistWithNameException>();
        var facade = Build();

        // When
        await facade
            .SetContext(Guid.NewGuid(), _validAzureDevOps)
            .FromProject(_project)
            .GetRepositoriesAsync();

        // Then
        Logger.ReceivedWithAnyArgs().Log(
            LogLevel.Error,
            0,
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    private AzureFacade Build() => new(Logger, ConnectionFactory);
}
